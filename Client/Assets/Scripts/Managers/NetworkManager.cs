using Google.Protobuf;
using NetworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager
{
    public string AccountId { get; set; }
    public string Token { get; set; }
	public string Name { get; set; }

    ServerSession _session;


    public void Send(IMessage packet)
    {
        _session.Send(packet);
    }

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

	public void ConnectToLobbyServer(ChannelInfo info)
	{
		_session = new ServerSession();
		IPAddress ipAddr = IPAddress.Parse(info.IpAddress);
		IPEndPoint endPoint = new IPEndPoint(ipAddr, info.Port);

		Connector connector = new Connector();
		connector.Connect(endPoint, () => { return _session; }, 1);
	}

	public void ConnectToGameServer(ChannelInfo info)
	{
		_session = new ServerSession();
		IPAddress ipAddr = IPAddress.Parse(info.IpAddress);
		IPEndPoint endPoint = new IPEndPoint(ipAddr, info.Port);

		Connector connector = new Connector();
		connector.Connect(endPoint, () => { return _session; }, 1);
	}

	public void DisconnectSession()
    {
		_session.Disconnect();
    }
}
