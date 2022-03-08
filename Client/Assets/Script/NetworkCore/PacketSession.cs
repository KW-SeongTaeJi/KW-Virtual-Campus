using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore
{
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;


        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processSize = 0;

            while (true)
            {
                // [size(2)][ ... ][size(2)][ ... ][size(2)][ ... ]

                if (buffer.Count < HeaderSize)
                    break;

                ushort packetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < packetSize)
                    break;

                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, packetSize));

                processSize += packetSize;

                // pull remains data to first index of buffer
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + packetSize, buffer.Count - packetSize);
            }

            return processSize;
        }

        // function when recevie packet
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }
}
