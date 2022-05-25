using Cinemachine;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

partial class PacketHandler
{
	public static void S_ConnectedHandler(PacketSession session, IMessage packet)
	{
		C_EnterGame enterGamePacket = new C_EnterGame()
		{
			AccountId = Managers.Network.AccountId,
		};
		Managers.Network.Send(enterGamePacket);
	}

	public static void S_PingHandler(PacketSession session, IMessage packet)
	{
		C_Pong pongPacket = new C_Pong();
		Debug.Log("[Server] PingCheck");
		Managers.Network.Send(pongPacket);
	}

	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = (S_EnterGame)packet;

		// Instantiate my player
		GameObject myPlayer = Managers.Object.Add(enterGamePacket.MyPlayer, myPlayer: true);
		for (int i = 0; i < enterGamePacket.Friends.Count; i++)
        {
			FriendInfo friend = new FriendInfo()
			{
				Name = enterGamePacket.Friends[i].Name,
				HairType = enterGamePacket.Friends[i].HairType,
				FaceType = enterGamePacket.Friends[i].FaceType,
				JacketType = enterGamePacket.Friends[i].JacketType,
				HairColor = enterGamePacket.Friends[i].HairColor,
				FaceColor_X = enterGamePacket.Friends[i].FaceColor.X,
				FaceColor_Y = enterGamePacket.Friends[i].FaceColor.Y,
				FaceColor_Z = enterGamePacket.Friends[i].FaceColor.Z
			};
			Managers.Object.MyPlayer.Friends.Add(friend.Name, friend);
        }
		((UI_GameScene)Managers.UI.SceneUI).SetFriendList();

		// Set player follow Camera
		GameObject gameObject = Managers.Resource.Instantiate("Object/MyPlayerFollowCamera");
		CinemachineVirtualCamera followCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
		followCamera.Follow = myPlayer.FindChild("PlayerCameraRoot").transform;
	}

	public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
	{
		S_LeaveGame leaveGamePacket = packet as S_LeaveGame;

		Managers.Object.Clear();
	}

	public static void S_SpawnHandler(PacketSession session, IMessage packet)
	{
		S_Spawn spawnPacket = (S_Spawn)packet;

		UI_Scene sceneUI = Managers.UI.SceneUI;
		foreach (ObjectInfo obj in spawnPacket.Objects)
		{
			Managers.Object.Add(obj);
			if (Managers.SceneLoad.CurrentScene.SceneType == Define.Scene.Game)
			{
				if (((UI_GameScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
				{
					((UI_GameScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
				}
			}
            else
			{
				if (((UI_IndoorScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
				{
					((UI_IndoorScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
				}
			}
		}
	}

	public static void S_DespawnHandler(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;

		Managers.Object.Remove(despawnPacket.ObjectId, despawnPacket.Name);

		UI_Scene sceneUI = Managers.UI.SceneUI;
		if (Managers.SceneLoad.CurrentScene.SceneType == Define.Scene.Game)
		{
			if (((UI_GameScene)sceneUI).FriendListSlots.ContainsKey(despawnPacket.Name))
			{
				((UI_GameScene)sceneUI).FriendListSlots[despawnPacket.Name].SetOffline();
			}
		}
        else
        {
			if (((UI_IndoorScene)sceneUI).FriendListSlots.ContainsKey(despawnPacket.Name))
			{
				((UI_IndoorScene)sceneUI).FriendListSlots[despawnPacket.Name].SetOffline();
			}
		}
	}

	public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
		S_Move movePacket = (S_Move)packet;

		GameObject gameObject = Managers.Object.FindById(movePacket.ObjectId);
		if (gameObject == null)
			return;

		PlayerController player = gameObject.GetComponent<PlayerController>();
		if (player == null)
			return;

		/* Set property used for move sync */
		// For player Move() And JumpAndGravity()
		player.TargetSpeed = movePacket.TargetSpeed;
		player.TargetRotation = movePacket.TargetRotation;
		player.Jump = movePacket.Jump;
		// For player SyncPosition()
		player.Sync = true;
		player.Position = new Vector3(movePacket.Position.X, movePacket.Position.Y, movePacket.Position.Z);
		player.RotationY = movePacket.RotationY;
	}

	public static void S_ChatHandler(PacketSession session, IMessage packet)
    {
		S_Chat chatPacket = (S_Chat)packet;

		GameObject gameObject = Managers.Object.FindById(chatPacket.ObjectId);
		if (gameObject == null)
			return;
		
		// outdoor
		if (Managers.Object.MyPlayer != null)
		{
			PlayerCanvasController playerCanvas = gameObject.FindChild<Canvas>().GetComponent<PlayerCanvasController>();
			playerCanvas.OnEnterChat(chatPacket.Message);
		}
		// indoor
		else
		{
			IndoorPlayerCanvasController playerCanvas = gameObject.FindChild<Canvas>().GetComponent<IndoorPlayerCanvasController>();
			playerCanvas.OnEnterChat(chatPacket.Message);
		}
	}
	
	public static void S_EmotionHandler(PacketSession session, IMessage packet)
    {
		S_Emotion emotionPacket = (S_Emotion)packet;

		GameObject gameObject = Managers.Object.FindById(emotionPacket.ObjectId);
		if (gameObject == null)
			return;

		PlayerController player = gameObject.GetComponent<PlayerController>();
		if (player == null)
			return;

		player.SetEmotionAnimation(emotionPacket.EmotionNum);
	}

	public static void S_EnterIndoorHandler(PacketSession session, IMessage packet)
    {
		S_EnterIndoor enterIndoorPacket = (S_EnterIndoor)packet;

		// Instantiate my player
		GameObject myPlayer = Managers.Object.AddIndoor(enterIndoorPacket.MyPlayer, myPlayer: true);
		for (int i = 0; i < enterIndoorPacket.Friends.Count; i++)
		{
			FriendInfo friend = new FriendInfo()
			{
				Name = enterIndoorPacket.Friends[i].Name,
				HairType = enterIndoorPacket.Friends[i].HairType,
				FaceType = enterIndoorPacket.Friends[i].FaceType,
				JacketType = enterIndoorPacket.Friends[i].JacketType,
				HairColor = enterIndoorPacket.Friends[i].HairColor,
				FaceColor_X = enterIndoorPacket.Friends[i].FaceColor.X,
				FaceColor_Y = enterIndoorPacket.Friends[i].FaceColor.Y,
				FaceColor_Z = enterIndoorPacket.Friends[i].FaceColor.Z
			};
			Managers.Object.MyIndoorPlayer.Friends.Add(friend.Name, friend);
		}
		((UI_IndoorScene)Managers.UI.SceneUI).SetFriendList();
	}

	public static void S_SpawnIndoorHandler(PacketSession session, IMessage packet)
    {
		S_SpawnIndoor spawnIndoorPacket = (S_SpawnIndoor)packet;

		UI_Scene sceneUI = Managers.UI.SceneUI;
		foreach (ObjectInfo obj in spawnIndoorPacket.Objects)
		{
			Managers.Object.AddIndoor(obj);
			if (Managers.SceneLoad.CurrentScene.SceneType == Define.Scene.Game)
			{
				if (((UI_GameScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
				{
					((UI_GameScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
				}
			}
			else
			{
				if (((UI_IndoorScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
				{
					((UI_IndoorScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
				}
			}
		}
	}

	public static void S_DespawnIndoorHandler(PacketSession session, IMessage packet)
	{
		S_DespawnIndoor despawnIndoorPacket = (S_DespawnIndoor)packet;

		Managers.Object.Remove(despawnIndoorPacket.ObjectId, despawnIndoorPacket.Name);

		UI_Scene sceneUI = Managers.UI.SceneUI;
		if (Managers.SceneLoad.CurrentScene.SceneType == Define.Scene.Game)
		{
			if (((UI_GameScene)sceneUI).FriendListSlots.ContainsKey(despawnIndoorPacket.Name))
			{
				((UI_GameScene)sceneUI).FriendListSlots[despawnIndoorPacket.Name].SetOffline();
			}
		}
		else
		{
			if (((UI_IndoorScene)sceneUI).FriendListSlots.ContainsKey(despawnIndoorPacket.Name))
			{
				((UI_IndoorScene)sceneUI).FriendListSlots[despawnIndoorPacket.Name].SetOffline();
			}
		}
	}

	public static void S_MoveIndoorHandler(PacketSession session, IMessage packet)
    {
		S_MoveIndoor moveIndoorPacket = (S_MoveIndoor)packet;

		GameObject gameObject = Managers.Object.FindById(moveIndoorPacket.ObjectId);
		if (gameObject == null)
			return;

		IndoorPlayerController player = gameObject.GetComponent<IndoorPlayerController>();
		if (player == null)
			return;

		/* Set property used for move sync */
		// For player Move()
		player.MoveX = moveIndoorPacket.MoveX;
		// For player SyncPosition()
		player.Sync = true;
		player.PosX = moveIndoorPacket.PosX;
	}
}
