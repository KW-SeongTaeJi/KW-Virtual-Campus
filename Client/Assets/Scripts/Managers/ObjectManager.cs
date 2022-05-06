using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
	public PlayerInfo MyPlayer { get; set; }
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();


    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

	public GameObject Add(ObjectInfo info, bool myPlayer = false)
	{
		if (MyPlayer != null && MyPlayer.Id == info.ObjectId)
			return null;

		if (_objects.ContainsKey(info.ObjectId))
			return null;

		GameObjectType objectType = GetObjectTypeById(info.ObjectId);
		if (objectType == GameObjectType.Player)
		{
			if (myPlayer)
			{
				GameObject gameObject = Managers.Resource.Instantiate("Object/MyGamePlayer");
				gameObject.transform.position = new Vector3(info.Position.X, info.Position.Y, info.Position.Z);
				gameObject.transform.rotation = Quaternion.Euler(0, info.RotationY, 0);
				_objects.Add(info.ObjectId, gameObject);

				MyPlayer = gameObject.GetComponent<PlayerInfo>();
				MyPlayer.Id = info.ObjectId;
				MyPlayer.Name = info.PlayerInfo.Name;
				MyPlayer.HairType = info.PlayerInfo.HairType;
				MyPlayer.FaceType = info.PlayerInfo.FaceType;
				MyPlayer.JacketType = info.PlayerInfo.JacketType;
				MyPlayer.HairColor = info.PlayerInfo.HairColor;
				MyPlayer.FaceColor_X = info.PlayerInfo.FaceColor.X;
				MyPlayer.FaceColor_Y = info.PlayerInfo.FaceColor.Y;
				MyPlayer.FaceColor_Z = info.PlayerInfo.FaceColor.Z;

				return gameObject;
			}
			else
			{
				GameObject gameObject = Managers.Resource.Instantiate("Object/GamePlayer");
				gameObject.transform.position = new Vector3(info.Position.X, info.Position.Y, info.Position.Z);
				gameObject.transform.rotation = Quaternion.Euler(0, info.RotationY, 0);
				_objects.Add(info.ObjectId, gameObject);

				PlayerInfo player = gameObject.GetComponent<PlayerInfo>();
				player.Id = info.ObjectId;
				player.Name = info.PlayerInfo.Name;
				player.HairType = info.PlayerInfo.HairType;
				player.FaceType = info.PlayerInfo.FaceType;
				player.JacketType = info.PlayerInfo.JacketType;
				player.HairColor = info.PlayerInfo.HairColor;
				player.FaceColor_X = info.PlayerInfo.FaceColor.X;
				player.FaceColor_Y = info.PlayerInfo.FaceColor.Y;
				player.FaceColor_Z = info.PlayerInfo.FaceColor.Z;

				return gameObject;
			}
		}

		return null;
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
