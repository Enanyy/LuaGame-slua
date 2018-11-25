using System;
using System.Collections.Generic;


namespace Network
{
    public class NetPacket
    {
        public const int DEFAULT_POWER = 5; //2^5 = 32
        static void ReAlloc(ref byte[] ba, int pos, int size)
        {
            if(ba == null)
            {
                ba = new byte[ReAllocSize(size)];
            }
            else if (ba.Length < (pos + size))
            {
                Array.Resize<byte>(ref ba, (int)(ReAllocSize(ba.Length + size)));
            }
        }
        static int ReAllocSize(int size)
        {
            int resize = 2;
            int i = DEFAULT_POWER;
            while ((resize = (int)Math.Pow(2, i)) < size) i++;
            return resize;
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
        public NetPacket()
        {
            readPosition = 0;
            writePosition = 0;
        }
        public NetPacket(byte[] data)
        {
            SetData(data);
        }

        public void SetData(byte[] _data)
        {
            readPosition = 0;
            writePosition = 0;
            WriteBytes(_data);
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
            Array.Copy(mData, oldPos, bytes, 0, len);
            return bytes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
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
