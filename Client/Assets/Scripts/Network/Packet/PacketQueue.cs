using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketMessage
{
    public ushort Id { get; set; }
    public IMessage Packet { get; set; }
}

public class PacketQueue
{
    static PacketQueue _instance = new PacketQueue();
    public static PacketQueue Instance { get { return _instance; } }

    object _lock = new object();
    Queue<PacketMessage> _packetQueue = new Queue<PacketMessage>();

    
    public void Push(ushort id, IMessage packet)
    {
        lock (_lock)
        {
            _packetQueue.Enqueue(new PacketMessage() { Id = id, Packet = packet });
        }
    }

    public PacketMessage Pop()
    {
        lock (_lock)
        {
            if (_packetQueue.Count == 0)
                return null;

            return _packetQueue.Dequeue();
        }
    }

    public List<PacketMessage> PopAll()
    {
        List<PacketMessage> list = new List<PacketMessage>();

        lock (_lock)
        {
            while (_packetQueue.Count > 0)
                list.Add(_packetQueue.Dequeue());
        }

        return list;
    }
}