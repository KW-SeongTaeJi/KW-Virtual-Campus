using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PacketHandler
{
	public static void L_ConnectedHandler(PacketSession session, IMessage packet)
	{
		B_EnterLobby enterPacket = new B_EnterLobby();
		enterPacket.AccountId = Managers.Network.AccountId;
		enterPacket.Token = Managers.Network.Token;
		Managers.Network.Send(enterPacket);
	}

	public static void L_PingHandler(PacketSession session, IMessage packet)
	{
		B_Pong pongPacket = new B_Pong();
		Debug.Log("[Server] PingCheck");
		Managers.Network.Send(pongPacket);
	}

	public static void L_EnterLobbyHandler(PacketSession session, IMessage packet)
	{
		L_EnterLobby enterPacket = (L_EnterLobby)packet;

		if (enterPacket.AccessOk == false)
        {
			Managers.Network.DisconnectSession();
		}
		Debug.Log("[Server] TokenMatch");
	}
}