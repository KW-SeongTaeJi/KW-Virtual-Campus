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

		Managers.Instance.HandleEnterLobby(enterPacket);
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

	public static void L_SaveInfoHandler(PacketSession session, IMessage packet)
	{
		L_SaveInfo saveInfoPacket = (L_SaveInfo)packet;

		UI_LobbyScene lobbyScene = (UI_LobbyScene)Managers.UI.SceneUI;
		lobbyScene.InfoPopup.OnRecvSaveInfoPacket(saveInfoPacket);
	}

	public static void L_FriendListHandler(PacketSession session, IMessage packet)
	{
		L_FriendList friendListPacket = (L_FriendList)packet;

		UI_LobbyScene lobbyScene = (UI_LobbyScene)Managers.UI.SceneUI;
		lobbyScene.FriendPopup.OnRecvFriendListPacket(friendListPacket);
	}

	public static void L_AddFriendHandler(PacketSession session, IMessage packet)
    {
		L_AddFriend addFriendPacket = (L_AddFriend)packet;
		UI_LobbyScene lobbyScene = (UI_LobbyScene)Managers.UI.SceneUI;
		lobbyScene.FriendPopup.FriendAddPopup.OnRecvAddFriendPacket(addFriendPacket);
	}

	public static void L_FriendRequestListHandler(PacketSession session, IMessage packet)
    {
		L_FriendRequestList friendRequestPacket = (L_FriendRequestList)packet;
		UI_LobbyScene lobbyScene = (UI_LobbyScene)Managers.UI.SceneUI;
		lobbyScene.FriendPopup.FriendRequestPopup.OnRecvFriendRequestListPacket(friendRequestPacket);
	}

	public static void L_AcceptFriendHandler(PacketSession session, IMessage packet)
    {
		L_AcceptFriend acceptFriendPacket = (L_AcceptFriend)packet;
		UI_LobbyScene lobbyScene = (UI_LobbyScene)Managers.UI.SceneUI;
		lobbyScene.FriendPopup.FriendRequestPopup.ReqeustSlots[acceptFriendPacket.Friend.Name].OnRecvAcceptFriendPacket(acceptFriendPacket);
	}

	public static void L_DeleteFriendHandler(PacketSession session, IMessage packet)
    {
		L_DeleteFriend deleteFriendPacket = (L_DeleteFriend)packet;

		if (deleteFriendPacket.Success == false)
        {
			return;
        }

		UI_LobbyScene lobbyScene = (UI_LobbyScene)Managers.UI.SceneUI;
		lobbyScene.FriendPopup.FriendSlots[deleteFriendPacket.FriendName].OnRecvDeleteFriendPacket();
	}
}