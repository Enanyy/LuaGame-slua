
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
namespace BTree
{
    public class BTSerialization
    {
        public static TreeConfig ReadBinary(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                TreeConfig bTree = binaryFormatter.Deserialize(ms) as TreeConfig;
                ms.Close();
                ms.Dispose();
                return bTree;
            }
        }

       
        public static TreeConfig ReadXML(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                XmlSerializer reader = new XmlSerializer(typeof(TreeConfig));
                StreamReader file = new StreamReader(ms);
                TreeConfig btree = reader.Deserialize(file) as TreeConfig;
                file.Close();
                ms.Close();
                ms.Dispose();
                return btree;
            }
        }
    }
}
