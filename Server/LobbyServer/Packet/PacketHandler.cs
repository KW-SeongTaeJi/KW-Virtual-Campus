using Google.Protobuf;
using Google.Protobuf.Protocol;
using LobbyServer;
using NetworkCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{	
	public static void B_PongHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		clientSession.HandlePong();
	}

	public static void B_EnterLobbyHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		B_EnterLobby enterLobbyPacket = (B_EnterLobby)packet;
		clientSession.HandleEnterLobby(enterLobbyPacket);
	}

	public static void B_SaveCustermizeHandler(PacketSession session, IMessage packet)
    {
		ClientSession clientSession = (ClientSession)session;
		B_SaveCustermize custermizePacket = (B_SaveCustermize)packet;

		Lobby lobby = MainLogic.Instance.Lobby;
		lobby.PushQueue(lobby.HandleCustermize, clientSession, custermizePacket);
	}
}