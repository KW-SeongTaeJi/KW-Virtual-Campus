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

	public static void B_SaveInfoHandler(PacketSession session, IMessage packet)
    {
		ClientSession clientSession = (ClientSession)session;
		B_SaveInfo infoPacket = (B_SaveInfo)packet;
		Lobby lobby = MainLogic.Instance.Lobby;
		lobby.PushQueue(lobby.HandleSaveInfo, clientSession, infoPacket);
	}

	public static void B_FriendListHandler(PacketSession session, IMessage packet)
    {
		ClientSession clientSession = (ClientSession)session;
		Lobby lobby = MainLogic.Instance.Lobby;
		lobby.PushQueue(lobby.HandleFriendList, clientSession);
	}		

	public static void B_AddFriendHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		B_AddFriend addFriendPacket = (B_AddFriend)packet;
		Lobby lobby = MainLogic.Instance.Lobby;
		lobby.PushQueue(lobby.HandleAddFriend, clientSession, addFriendPacket);
	}		

	public static void B_FriendRequestListHandler(PacketSession session, IMessage packet)
    {
		ClientSession clientSession = (ClientSession)session;
		B_FriendRequestList friendRequestListPacket = (B_FriendRequestList)packet;
		Lobby lobby = MainLogic.Instance.Lobby;
		lobby.PushQueue(lobby.HandleFriendRequestList, clientSession, friendRequestListPacket);
	}

	public static void B_AcceptFriendHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = (ClientSession)session;
		B_AcceptFriend acceptFriendPacket = (B_AcceptFriend)packet;
		Lobby lobby = MainLogic.Instance.Lobby;
		lobby.PushQueue(lobby.HandleAcceptFriend, clientSession, acceptFriendPacket);
	}

	public static void B_DeleteFriendHandler(PacketSession session, IMessage packet)
    {
		ClientSession clientSession = (ClientSession)session;
		B_DeleteFriend deleteFriendPacket = (B_DeleteFriend)packet;
		Lobby lobby = MainLogic.Instance.Lobby;
		lobby.PushQueue(lobby.HandleDeleteFriend, clientSession, deleteFriendPacket);
	}
}