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
        private int mPosition;
        public byte[] data { get { return mData; } }

        public int Length
        {
            get
            {
                return mData.Length;
            }
        }

        public int Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        public NetPacket(byte[] data)
        {
            SetData(data);
        }

        public void SetData(byte[] data)
        {
            mData = data;
            mPosition = 0;
        }
        public int ReadInt()
        {
            int oldPos = mPosition;
            mPosition += 4;
            return BitConverter.ToInt32(mData, oldPos);
        }

        public uint ReadUInt()
        {
            int oldPos = mPosition;
            mPosition += 4;
            return BitConverter.ToUInt32(mData, oldPos);
        }

        public short ReadShort()
        {
            return ReadInt16();
        }

        public ushort ReadUShort()
        {
            return ReadUInt16();
        }

        public short ReadInt16()
        {
            int oldPos = mPosition;
            mPosition += 2;
            return BitConverter.ToInt16(mData, oldPos);
        }

        public ushort ReadUInt16()
        {
            int oldPos = mPosition;
            mPosition += 2;
            return BitConverter.ToUInt16(mData, oldPos);
        }

        public Int64 ReadInt64()
        {
            int oldPos = mPosition;
            mPosition += 8;
            return BitConverter.ToInt64(mData, oldPos);
        }

        public UInt64 ReadUInt64()
        {
            int oldPos = mPosition;
            mPosition += 8;
            return BitConverter.ToUInt64(mData, oldPos);
        }

        public void WriteInt(int v)
        {
            ReAlloc(ref mData, mPosition, 4);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, mPosition, 4);
            mPosition += 4;
        }
        public void WriteUInt(uint v)
        {
            ReAlloc(ref mData, mPosition, 4);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, mPosition, 4);
            mPosition += 4;
        }
        public void WriteUShort(ushort v)
        {
            ReAlloc(ref mData, mPosition, 2);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, mPosition, 2);
            mPosition += 2;
        }

        public void WriteShort(short v)
        {
            ReAlloc(ref mData, mPosition, 2);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, mPosition, 2);
            mPosition += 2;
        }
        public void WriteInt64(Int64 v)
        {
            ReAlloc(ref mData, mPosition, 8);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, mPosition, 8);
            mPosition += 8;
        }

        public void WriteUInt64(UInt64 v)
        {
            ReAlloc(ref mData, mPosition, 8);
            Array.Copy(BitConverter.GetBytes(v), 0, mData, mPosition, 8);
            mPosition += 8;
        }

        public void Clear()
        {
            mPosition = 0;
        }
    }
}
