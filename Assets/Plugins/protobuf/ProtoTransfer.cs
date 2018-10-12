using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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


    public static T DeserializeProtoBuf<T>(byte[] data) where T : class, ProtoBuf.IExtensible
    {
        if (data == null)
        {
            return null;
        }
        using (MemoryStream ms = new MemoryStream(data))
        {
            T t = ProtoBuf.Serializer.Deserialize<T>(ms);
            return t;
        }
    }

    public static object DeserializeProtoBuf(byte[] data, Type type)
    {
        if (data == null)
        {
            return null;
        }

        using (MemoryStream ms = new MemoryStream(data))
        {
            return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(ms, null, type);
        }
    }
}

