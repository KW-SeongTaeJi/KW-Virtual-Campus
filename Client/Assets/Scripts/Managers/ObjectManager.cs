using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
	public MyPlayerController MyPlayer { get; set; }
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();


    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

	public void Add(ObjectInfo info, bool myPlayer = false)
	{
		if (MyPlayer != null && MyPlayer.Id == info.ObjectId)
			return;

		if (_objects.ContainsKey(info.ObjectId))
			return;

		GameObjectType objectType = GetObjectTypeById(info.ObjectId);
		if (objectType == GameObjectType.Player)
		{
			if (myPlayer)
			{
				GameObject gameObject = Managers.Resource.Instantiate("Object/Player");
				gameObject.name = info.Name;
				_objects.Add(info.ObjectId, gameObject);

				MyPlayer = gameObject.GetComponent<MyPlayerController>();
				MyPlayer.Id = info.ObjectId;
			}
			else
			{
				GameObject gameObject = Managers.Resource.Instantiate("Object/Player");
				gameObject.name = info.Name;
				_objects.Add(info.ObjectId, gameObject);

				PlayerController player = gameObject.GetComponent<PlayerController>();
				player.Id = info.ObjectId;
			}
		}
	}

	public GameObject FindById(int id)
	{
		GameObject gameObject = null;
		_objects.TryGetValue(id, out gameObject);
		return gameObject;
	}

	public void Remove(int id)
	{
		if (_objects.ContainsKey(id) == false)
			return;

		GameObject gameObject = FindById(id);
		if (gameObject == null)
			return;

		_objects.Remove(id);
		Managers.Resource.Destroy(gameObject);
	}

	public void Clear()
	{
		foreach (GameObject obj in _objects.Values)
			Managers.Resource.Destroy(obj);

		_objects.Clear();
		MyPlayer = null;
	}
}
