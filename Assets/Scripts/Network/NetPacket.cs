using System;
using System.Collections.Generic;


namespace Network
{
    public class NetPacket
    {
        static void ReAlloc(ref byte[] ba, int pos, int size)
        {
            if (ba.Length < (pos + size))
            {
                Array.Resize<byte>(ref ba, (int)(ba.Length + size + 1024));
            }
        }

        private byte[] mData;
        public int writePosition { get; private set; }
        public int readPosition { get; private set; }
        public byte[] data { get { return mData; } }

        public int Length
        {
            get
            {
                return mData.Length;
            }
        }

        public NetPacket(byte[] data)
        {
            SetData(data);
        }

        public void SetData(byte[] data)
        {
            mData = data;
            writePosition = 0;
            readPosition = 0;
        }
        public int ReadInt()
        {
            int oldPos = readPosition;
            readPosition += 4;
            return BitConverter.ToInt32(mData, oldPos);
        }    
        public void WriteInt(int v)
        {
            ReAlloc(ref mData, writePosition, 4);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, writePosition, 4);
            writePosition += 4;
        }
        public uint ReadUInt()
        {
            int oldPos = readPosition;
            readPosition += 4;
            return BitConverter.ToUInt32(mData, oldPos);
        }
        public void WriteUInt(uint v)
        {
            ReAlloc(ref mData, writePosition, 4);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, writePosition, 4);
            writePosition += 4;
        }

        public short ReadShort()
        {
            return ReadInt16();
        }

        public void WriteShort(short v)
        {
            ReAlloc(ref mData, writePosition, 2);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, writePosition, 2);
            writePosition += 2;
        }

        public ushort ReadUShort()
        {
            return ReadUInt16();
        }

        public void WriteUShort(ushort v)
        {
            ReAlloc(ref mData, writePosition, 2);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, writePosition, 2);
            writePosition += 2;
        }

       
        public short ReadInt16()
        {
            int oldPos = readPosition;
            readPosition += 2;
            return BitConverter.ToInt16(mData, oldPos);
        }

        public ushort ReadUInt16()
        {
            int oldPos = readPosition;
            readPosition += 2;
            return BitConverter.ToUInt16(mData, oldPos);
        }

        public Int64 ReadInt64()
        {
            int oldPos = readPosition;
            readPosition += 8;
            return BitConverter.ToInt64(mData, oldPos);
        }
        public void WriteInt64(Int64 v)
        {
            ReAlloc(ref mData, writePosition, 8);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, writePosition, 8);
            writePosition += 8;
        }
        public UInt64 ReadUInt64()
        {
            int oldPos = readPosition;
            readPosition += 8;
            return BitConverter.ToUInt64(mData, oldPos);
        }
        public void WriteUInt64(UInt64 v)
        {
            ReAlloc(ref mData, writePosition, 8);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, writePosition, 8);
            writePosition += 8;
        }

        public byte[] ReadBytes()
        {
            int len = ReadInt();
            int oldPos = readPosition;
            readPosition += len;
            byte[] bytes = new byte[len];
            for (int i = 0; i < len; ++i)
            {
                bytes[i] = mData[oldPos + i];
            }
            return bytes;
        }

        public void WriteBytes(byte[] v)
        {
            int len = v.Length;
            WriteInt(len);
            ReAlloc(ref mData, writePosition, len);
            v.CopyTo(mData, writePosition);
            writePosition += len;
        }

        public void Clear()
        {
            writePosition = 0;
            readPosition = 0;
        }
    }
}
