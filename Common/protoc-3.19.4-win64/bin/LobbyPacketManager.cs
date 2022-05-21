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
		_onRecv.Add((ushort)MsgId.BPong, MakePacket<B_Pong>);
		_handler.Add((ushort)MsgId.BPong, PacketHandler.B_PongHandler);		
		_onRecv.Add((ushort)MsgId.BEnterLobby, MakePacket<B_EnterLobby>);
		_handler.Add((ushort)MsgId.BEnterLobby, PacketHandler.B_EnterLobbyHandler);		
		_onRecv.Add((ushort)MsgId.BSaveCustermize, MakePacket<B_SaveCustermize>);
		_handler.Add((ushort)MsgId.BSaveCustermize, PacketHandler.B_SaveCustermizeHandler);		
		_onRecv.Add((ushort)MsgId.BSaveInfo, MakePacket<B_SaveInfo>);
		_handler.Add((ushort)MsgId.BSaveInfo, PacketHandler.B_SaveInfoHandler);		
		_onRecv.Add((ushort)MsgId.BFriendList, MakePacket<B_FriendList>);
		_handler.Add((ushort)MsgId.BFriendList, PacketHandler.B_FriendListHandler);		
		_onRecv.Add((ushort)MsgId.BAddFriend, MakePacket<B_AddFriend>);
		_handler.Add((ushort)MsgId.BAddFriend, PacketHandler.B_AddFriendHandler);		
		_onRecv.Add((ushort)MsgId.BFriendRequestList, MakePacket<B_FriendRequestList>);
		_handler.Add((ushort)MsgId.BFriendRequestList, PacketHandler.B_FriendRequestListHandler);		
		_onRecv.Add((ushort)MsgId.BAcceptFriend, MakePacket<B_AcceptFriend>);
		_handler.Add((ushort)MsgId.BAcceptFriend, PacketHandler.B_AcceptFriendHandler);		
		_onRecv.Add((ushort)MsgId.BDeleteFriend, MakePacket<B_DeleteFriend>);
		_handler.Add((ushort)MsgId.BDeleteFriend, PacketHandler.B_DeleteFriendHandler);
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