using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectManager
{
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    Dictionary<string, PlayerInfo> _players = new Dictionary<string, PlayerInfo>();

	public Player MyPlayer { get; set; }
	public IndoorPlayer MyIndoorPlayer { get; set; }


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

		/* Add player instance */
		if (objectType == GameObjectType.Player)
		{
			if (_players.ContainsKey(info.PlayerInfo.Name) == false)
				_players.Add(info.PlayerInfo.Name, info.PlayerInfo);

			if (myPlayer)
			{
				GameObject gameObject = Managers.Resource.Instantiate("Object/MyGamePlayer");
				gameObject.transform.position = new Vector3(info.Position.X, info.Position.Y, info.Position.Z);
				gameObject.transform.rotation = Quaternion.Euler(0, info.RotationY, 0);
				_objects.Add(info.ObjectId, gameObject);

				MyPlayer = gameObject.GetComponent<Player>();
				MyPlayer.Id = info.ObjectId;
				MyPlayer.Name = info.PlayerInfo.Name;
				MyPlayer.HairType = info.PlayerInfo.HairType;
				MyPlayer.FaceType = info.PlayerInfo.FaceType;
				MyPlayer.JacketType = info.PlayerInfo.JacketType;
				MyPlayer.HairColor = info.PlayerInfo.HairColor;
				MyPlayer.FaceColor_X = info.PlayerInfo.FaceColor.X;
				MyPlayer.FaceColor_Y = info.PlayerInfo.FaceColor.Y;
				MyPlayer.FaceColor_Z = info.PlayerInfo.FaceColor.Z;
				MyPlayer.Place = info.PlayerInfo.Place;

				gameObject.FindChild<TextMeshProUGUI>("NameText", recursive: true).text = MyPlayer.Name;

				UI_GameScene gameSceneUI = (UI_GameScene)Managers.UI.SceneUI;
				gameSceneUI.MyPlayerCanvas = gameObject.FindChild<Canvas>().GetComponent<PlayerCanvasController>();
				gameSceneUI.MyPlayerController = gameObject.GetComponent<MyPlayerController>();

				return gameObject;
			}
			else
			{
				if (MyPlayer != null)
				{
					if (info.PlayerInfo.Place == MyPlayer.Place)
					{
						GameObject gameObject = Managers.Resource.Instantiate("Object/GamePlayer");
						gameObject.transform.position = new Vector3(info.Position.X, info.Position.Y, info.Position.Z);
						gameObject.transform.rotation = Quaternion.Euler(0, info.RotationY, 0);
						_objects.Add(info.ObjectId, gameObject);

						Player player = gameObject.GetComponent<Player>();
						player.Id = info.ObjectId;
						player.Name = info.PlayerInfo.Name;
						player.HairType = info.PlayerInfo.HairType;
						player.FaceType = info.PlayerInfo.FaceType;
						player.JacketType = info.PlayerInfo.JacketType;
						player.HairColor = info.PlayerInfo.HairColor;
						player.FaceColor_X = info.PlayerInfo.FaceColor.X;
						player.FaceColor_Y = info.PlayerInfo.FaceColor.Y;
						player.FaceColor_Z = info.PlayerInfo.FaceColor.Z;
						player.Place = info.PlayerInfo.Place;

						gameObject.FindChild<TextMeshProUGUI>("NameText", recursive: true).text = player.Name;

						return gameObject;
					}
				}
			}
		}

		return null;
	}
	public GameObject AddIndoor(ObjectInfo info, bool myPlayer = false)
    {
		if (MyIndoorPlayer != null && MyIndoorPlayer.Id == info.ObjectId)
			return null;

		if (_objects.ContainsKey(info.ObjectId))
			return null;

		GameObjectType objectType = GetObjectTypeById(info.ObjectId);

		if (objectType == GameObjectType.Player)
		{
			if (_players.ContainsKey(info.PlayerInfo.Name) == false)
				_players.Add(info.PlayerInfo.Name, info.PlayerInfo);

			if (myPlayer)
			{
				GameObject gameObject = Managers.Resource.Instantiate("Object/MyIndoorPlayer");
				gameObject.transform.position = new Vector3(info.Position.X, info.Position.Y, info.Position.Z - _objects.Count);
				_objects.Add(info.ObjectId, gameObject);

				MyIndoorPlayer = gameObject.GetComponent<IndoorPlayer>();
				MyIndoorPlayer.FaceColor_X = info.PlayerInfo.FaceColor.X;
				MyIndoorPlayer.FaceColor_Y = info.PlayerInfo.FaceColor.Y;
				MyIndoorPlayer.FaceColor_Z = info.PlayerInfo.FaceColor.Z;
				MyIndoorPlayer.Id = info.ObjectId;
				MyIndoorPlayer.Name = info.PlayerInfo.Name;
				MyIndoorPlayer.HairType = info.PlayerInfo.HairType;
				MyIndoorPlayer.FaceType = info.PlayerInfo.FaceType;
				MyIndoorPlayer.JacketType = info.PlayerInfo.JacketType;
				MyIndoorPlayer.HairColor = info.PlayerInfo.HairColor;
				MyIndoorPlayer.Place = info.PlayerInfo.Place;
				MyIndoorPlayer.SetBodyColor();

				gameObject.FindChild<TextMeshProUGUI>("NameText", recursive: true).text = MyIndoorPlayer.Name;

				UI_IndoorScene indoorSceneUI = (UI_IndoorScene)Managers.UI.SceneUI;
				indoorSceneUI.MyPlayerCanvas = gameObject.FindChild<Canvas>().GetComponent<IndoorPlayerCanvasController>();

				return gameObject;
			}
            else
            {
				if (MyIndoorPlayer != null)
				{
					if (info.PlayerInfo.Place == MyIndoorPlayer.Place)
					{
						GameObject gameObject = Managers.Resource.Instantiate("Object/IndoorPlayer");
						gameObject.transform.position = new Vector3(info.Position.X, info.Position.Y, info.Position.Z - _objects.Count);
						_objects.Add(info.ObjectId, gameObject);

						IndoorPlayer player = gameObject.GetComponent<IndoorPlayer>();
						player.FaceColor_X = info.PlayerInfo.FaceColor.X;
						player.FaceColor_Y = info.PlayerInfo.FaceColor.Y;
						player.FaceColor_Z = info.PlayerInfo.FaceColor.Z;
						player.Id = info.ObjectId;
						player.Name = info.PlayerInfo.Name;
						player.HairType = info.PlayerInfo.HairType;
						player.FaceType = info.PlayerInfo.FaceType;
						player.JacketType = info.PlayerInfo.JacketType;
						player.HairColor = info.PlayerInfo.HairColor;
						player.Place = info.PlayerInfo.Place;
						player.SetBodyColor();

						gameObject.FindChild<TextMeshProUGUI>("NameText", recursive: true).text = player.Name;

						return gameObject;
					}
				}
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

	public PlayerInfo FindPlayerByName(string name)
    {
		PlayerInfo player = null;
		_players.TryGetValue(name, out player);
		return player;
    }

	public void Remove(int id, string name)
	{
		if (GetObjectTypeById(id) == GameObjectType.Player)
		{
			if (_players.ContainsKey(name) == false)
				return;
			_players.Remove(name);
		}

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
		_players.Clear();
		MyPlayer = null;
		MyIndoorPlayer = null;
	}
}
