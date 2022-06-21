using Google.Protobuf;
using NetworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager
{
	ServerSession _session;

	public string AccountId { get; set; }
    public string Token { get; set; }
	public string Name { get; set; }

	public ServerInfo LobbyServer { get; set; }
	public ServerInfo GameServer1 { get; set; }


	public void Update()
	{
		List<PacketMessage> list = PacketQueue.Instance.PopAll();
		foreach (PacketMessage packet in list)
		{
			Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
			if (handler != null)
				handler.Invoke(_session, packet.Packet);
		}
	}

	public void Send(IMessage packet)
	{
		_session.Send(packet);
	}

	public void ConnectToServer(ServerInfo server)
	{
		_session = new ServerSession();

		IPAddress ipAddr = IPAddress.Parse(server.IpAddress);
		IPEndPoint endPoint = new IPEndPoint(ipAddr, server.Port);
		Connector connector = new Connector();
		connector.Connect(endPoint, () => { return _session; }, 1);
	}
	public void ConnectToLobbyServer()
	{
		ConnectToServer(LobbyServer);
	}
	public void ConnectToGameServer1()
	{
		ConnectToServer(GameServer1);
	}

	public void DisconnectSession()
    {
		_session.Disconnect();
    }
}
