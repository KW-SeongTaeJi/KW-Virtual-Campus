using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;

public class ServerSession : PacketSession
{
    public int DummyId { get; set; }
    public float PosX { get; set; } = -48;
    public float PosY { get; set; } = 14;
    public float PosZ { get; set; }


    public void Send(IMessage packet)
    {
        string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
        MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

        ushort size = (ushort)packet.CalculateSize();
        byte[] sendBuffer = new byte[size + 4];
        Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
        Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
        Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

        Send(new ArraySegment<byte>(sendBuffer));
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        PacketManager.Instance.OnRecvPacket(this, buffer);
    }

    public override void OnConnected()
    {
        Console.WriteLine($"OnConnected");
    }

    public override void OnDisconnected()
    {
        Console.WriteLine($"OnDisconnected");
    }

    public override void OnSend()
    {

    }
}
