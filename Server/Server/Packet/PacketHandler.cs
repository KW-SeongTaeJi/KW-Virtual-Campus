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

		int playerId = clientSession.MyPlayer.Id;
		GameWorld gameWorld = GameLogic.Instance.GameWorld;
		gameWorld.PushQueue(gameWorld.HandleChat, playerId, chatPacket);
	}
}
