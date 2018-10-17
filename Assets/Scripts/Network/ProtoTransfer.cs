using Network;
using System;
using System.IO;
using UnityEngine;

namespace Network
{
    public static class ProtoTransfer
    {
        public static byte[] SerializeProtoBuf<T>(T data) where T : class, ProtoBuf.IExtensible
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(ms, data);
                byte[] bytes = ms.ToArray();

                ms.Close();

                return bytes;
            }
        }

        

        public static T DeserializeProtoBuf<T>(MemoryStream ms) where T : class, ProtoBuf.IExtensible
        {
            if(ms == null)
            {
                return null;
            }
           
            T t = ProtoBuf.Serializer.Deserialize<T>(ms);

            ms.Dispose();

            return t;
            
        }

        public static object DeserializeProtoBuf(MemoryStream ms, Type type)
        {
            if (ms == null)
            {
                return null;
            }
            
            object o = ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(ms, null, type);

            ms.Dispose();

            return o;
            
        }


    }
}
