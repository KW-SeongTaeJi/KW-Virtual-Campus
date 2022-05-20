using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.LConnected, MakePacket<L_Connected>);
		_handler.Add((ushort)MsgId.LConnected, PacketHandler.L_ConnectedHandler);		
		_onRecv.Add((ushort)MsgId.LPing, MakePacket<L_Ping>);
		_handler.Add((ushort)MsgId.LPing, PacketHandler.L_PingHandler);		
		_onRecv.Add((ushort)MsgId.LEnterLobby, MakePacket<L_EnterLobby>);
		_handler.Add((ushort)MsgId.LEnterLobby, PacketHandler.L_EnterLobbyHandler);		
		_onRecv.Add((ushort)MsgId.LSaveCustermize, MakePacket<L_SaveCustermize>);
		_handler.Add((ushort)MsgId.LSaveCustermize, PacketHandler.L_SaveCustermizeHandler);		
		_onRecv.Add((ushort)MsgId.LSaveInfo, MakePacket<L_SaveInfo>);
		_handler.Add((ushort)MsgId.LSaveInfo, PacketHandler.L_SaveInfoHandler);		
		_onRecv.Add((ushort)MsgId.LFriendList, MakePacket<L_FriendList>);
		_handler.Add((ushort)MsgId.LFriendList, PacketHandler.L_FriendListHandler);		
		_onRecv.Add((ushort)MsgId.LAddFriend, MakePacket<L_AddFriend>);
		_handler.Add((ushort)MsgId.LAddFriend, PacketHandler.L_AddFriendHandler);		
		_onRecv.Add((ushort)MsgId.LFriendRequestList, MakePacket<L_FriendRequestList>);
		_handler.Add((ushort)MsgId.LFriendRequestList, PacketHandler.L_FriendRequestListHandler);		
		_onRecv.Add((ushort)MsgId.LAcceptFriend, MakePacket<L_AcceptFriend>);
		_handler.Add((ushort)MsgId.LAcceptFriend, PacketHandler.L_AcceptFriendHandler);		
		_onRecv.Add((ushort)MsgId.LDeleteFriend, MakePacket<L_DeleteFriend>);
		_handler.Add((ushort)MsgId.LDeleteFriend, PacketHandler.L_DeleteFriendHandler);		
		_onRecv.Add((ushort)MsgId.SConnected, MakePacket<S_Connected>);
		_handler.Add((ushort)MsgId.SConnected, PacketHandler.S_ConnectedHandler);		
		_onRecv.Add((ushort)MsgId.SPing, MakePacket<S_Ping>);
		_handler.Add((ushort)MsgId.SPing, PacketHandler.S_PingHandler);		
		_onRecv.Add((ushort)MsgId.SEnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)MsgId.SEnterGame, PacketHandler.S_EnterGameHandler);		
		_onRecv.Add((ushort)MsgId.SLeaveGame, MakePacket<S_LeaveGame>);
		_handler.Add((ushort)MsgId.SLeaveGame, PacketHandler.S_LeaveGameHandler);		
		_onRecv.Add((ushort)MsgId.SSpawn, MakePacket<S_Spawn>);
		_handler.Add((ushort)MsgId.SSpawn, PacketHandler.S_SpawnHandler);		
		_onRecv.Add((ushort)MsgId.SDespawn, MakePacket<S_Despawn>);
		_handler.Add((ushort)MsgId.SDespawn, PacketHandler.S_DespawnHandler);		
		_onRecv.Add((ushort)MsgId.SMove, MakePacket<S_Move>);
		_handler.Add((ushort)MsgId.SMove, PacketHandler.S_MoveHandler);		
		_onRecv.Add((ushort)MsgId.SChat, MakePacket<S_Chat>);
		_handler.Add((ushort)MsgId.SChat, PacketHandler.S_ChatHandler);		
		_onRecv.Add((ushort)MsgId.SEmotion, MakePacket<S_Emotion>);
		_handler.Add((ushort)MsgId.SEmotion, PacketHandler.S_EmotionHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		if (CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}