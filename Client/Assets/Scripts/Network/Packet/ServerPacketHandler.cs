﻿using Cinemachine;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

partial class PacketHandler
{
	public static void S_ConnectedHandler(PacketSession session, IMessage packet)
	{
		C_EnterGame enterGamePacket = new C_EnterGame();
		enterGamePacket.AccountId = Managers.Network.AccountId;
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

		foreach (ObjectInfo obj in spawnPacket.Objects)
		{
			Managers.Object.Add(obj);
		}
	}

	public static void S_DespawnHandler(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;

		Managers.Object.Remove(despawnPacket.ObjectId);
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
		// For  player Move() And JumpAndGravity()
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

		PlayerCanvasController playerCanvas = gameObject.FindChild<Canvas>().GetComponent<PlayerCanvasController>();
		playerCanvas.OnEnterChat(chatPacket.Message);
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
}
