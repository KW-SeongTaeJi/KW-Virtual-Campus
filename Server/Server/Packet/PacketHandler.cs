using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using Server;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_PongHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		clientSession.HandlePong();
	}

	public static void C_EnterGameHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		C_EnterGame enterGamePacket = (C_EnterGame)packet;

		clientSession.HandleEnterGame(enterGamePacket);
	}

	public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		C_Move movePacket = (C_Move)packet;

		Player myPlayer = clientSession.MyPlayer;
		if (myPlayer == null)
			return;

		GameWorld gameWorld = GameLogic.Instance.GameWorld;
		gameWorld.PushQueue(gameWorld.HandleMove, myPlayer, movePacket);
	}

	public static void C_ChatHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		C_Chat chatPacket = (C_Chat)packet;

		GameWorld gameWorld = GameLogic.Instance.GameWorld;
		gameWorld.PushQueue(gameWorld.HandleChat, clientSession, chatPacket);
	}

	public static void C_EmotionHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		C_Emotion emotionPacket = (C_Emotion)packet;

		int playerId = clientSession.MyPlayer.Id;
		GameWorld gameWorld = GameLogic.Instance.GameWorld;
		gameWorld.PushQueue(gameWorld.HandleEmotion, playerId, emotionPacket);
	}

	public static void C_EnterIndoorHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		C_EnterIndoor enterIndoorPacket = (C_EnterIndoor)packet;

		GameWorld gameWorld = GameLogic.Instance.GameWorld;
		gameWorld.PushQueue(gameWorld.HandleEnterIndoor, clientSession, enterIndoorPacket);
	}

	public static void C_MoveIndoorHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		C_MoveIndoor moveIndoorPacket = (C_MoveIndoor)packet;

		GameWorld gameWorld = GameLogic.Instance.GameWorld;
		gameWorld.PushQueue(gameWorld.HandleMoveIndoor, clientSession, moveIndoorPacket);
	}

	public static void C_LeaveIndoorHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;

		GameWorld gameWorld = GameLogic.Instance.GameWorld;
		gameWorld.PushQueue(gameWorld.HandleLeaveIndoor, clientSession);
	}
}
