using System;
using System.Collections.Generic;


namespace Network
{
    public class NetPacket
    {
        private byte[] mData;
        public NetPacket(byte[] data)
        {
            mData = data;
        }

        public NetPacket(ushort lenght)
        {
            mData = new byte[lenght];
        }

        public byte[] GetBuffer()
        {
            return mData;
        }
    }
}
