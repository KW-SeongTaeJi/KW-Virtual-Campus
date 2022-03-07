using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore
{
    class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        int _readCur;
        int _writeCur;
        
        public int DataSize { get { return _writeCur - _readCur; } }
        public int FreeSize { get { return _buffer.Count - _writeCur; } }

        public ArraySegment<byte> ReadSegment
        {
            get
            {
                return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readCur, DataSize);
            }
        }
        public ArraySegment<byte> WriteSegment
        {
            get
            {
                return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writeCur, FreeSize);
            }
        }


        public RecvBuffer(int size)
        {
            _buffer = new ArraySegment<byte>(new byte[size], 0, size);
        }

        public bool OnRead(int bytes)
        {
            if (bytes > DataSize)
                return false;

            _readCur += bytes;
            return true;
        }

        public bool OnWrite(int bytes)
        {
            if (bytes > FreeSize)
                return false;

            _writeCur += bytes;
            return true;
        }

        public void Clean()
        {
            int dataSize = DataSize;
            if (dataSize == 0)
            {
                // no remains data in buffer
                _readCur = _writeCur = 0;
            }
            else
            {
                // pull remains data to first index of buffer
                Array.Copy(_buffer.Array, _buffer.Offset + _readCur, _buffer.Array, _buffer.Offset, dataSize);
                _readCur = 0;
                _writeCur = dataSize;
            }
        }
    }
}
