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
		enterPacket.Name = Managers.Network.Name;
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

		// Disconnect invalid Access
		if (enterPacket.AccessOk == false)
        {
			Managers.Network.DisconnectSession();
			return;
		}

		// Set player in lobby
		GameObject player = Managers.Resource.Instantiate("Object/LobbyPlayer");
		PlayerInfo info = player.GetComponent<PlayerInfo>();
		{
			info.HairType = enterPacket.HairType;
			info.FaceType = enterPacket.FaceType;
			info.JacketType = enterPacket.JacketType;
			info.HairColor = enterPacket.HairColor;
			info.FaceColor_X = enterPacket.FaceColor.X;
			info.FaceColor_Y = enterPacket.FaceColor.Y;
			info.FaceColor_Z = enterPacket.FaceColor.Z;
		}
		LobbyScene lobbyScene = (LobbyScene)Managers.SceneLoad.CurrentScene;
		lobbyScene.playerInfo = info;
	}

	public static void L_SaveCustermizeHandler(PacketSession session, IMessage packet)
    {
		L_SaveCustermize custermizePacket = (L_SaveCustermize)packet;

		if (custermizePacket.SaveOk == false)
        {
			return;
        }

		UI_LobbyScene lobbyScene = (UI_LobbyScene)Managers.UI.SceneUI;
		lobbyScene.CustermizePopup.OnRecvCustermizePacket();
    }
}