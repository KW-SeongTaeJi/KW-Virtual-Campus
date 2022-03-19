using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using Server;
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
}
