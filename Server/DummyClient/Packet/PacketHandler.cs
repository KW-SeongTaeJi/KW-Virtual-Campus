using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using System;
using System.Collections.Generic;
using System.Text;


class PacketHandler
{
	public static void S_ConnectedHandler(PacketSession session, IMessage packet)
	{
		C_EnterGame enterGamePacket = new C_EnterGame();
		ServerSession serverSession = (ServerSession)session;

		enterGamePacket.AccountId = $"dummy{serverSession.DummyId}";
		serverSession.Send(enterGamePacket);
	}
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = (S_EnterGame)packet;
	}

	public static void S_PingHandler(PacketSession session, IMessage packet)
	{
		C_Pong pongPacket = new C_Pong();
	}

	public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
	{
		S_LeaveGame leaveGamePacket = (S_LeaveGame)packet;
	}

	public static void S_SpawnHandler(PacketSession session, IMessage packet)
	{
		S_Spawn spawnPacket = (S_Spawn)packet;
	}

	public static void S_DespawnHandler(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;
	}

	public static void S_MoveHandler(PacketSession session, IMessage packet)
	{
		S_Move movePacket = (S_Move)packet;
	}

	public static void S_ChatHandler(PacketSession session, IMessage packet)
	{
		S_Chat chatPacket = (S_Chat)packet;
	}

	public static void S_EmotionHandler(PacketSession session, IMessage packet)
	{
		S_Emotion emotionPacket = (S_Emotion)packet;
	}

	public static void S_EnterIndoorHandler(PacketSession session, IMessage packet)
	{
		S_EnterIndoor enterIndoorPacket = (S_EnterIndoor)packet;
	}

	public static void S_SpawnIndoorHandler(PacketSession session, IMessage packet)
	{
		S_SpawnIndoor spawnIndoorPacket = (S_SpawnIndoor)packet;
	}

	public static void S_DespawnIndoorHandler(PacketSession session, IMessage packet)
	{
		S_DespawnIndoor despawnIndoorPacket = (S_DespawnIndoor)packet;
	}

	public static void S_MoveIndoorHandler(PacketSession session, IMessage packet)
	{
		S_MoveIndoor moveIndoorPacket = (S_MoveIndoor)packet;
	}
}