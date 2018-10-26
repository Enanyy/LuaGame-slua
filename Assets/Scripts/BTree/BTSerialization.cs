
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
namespace BTree
{
    public class BTSerialization
    {
        public static string configPath = "Assets/R/Config/Player/";

        public static void WriteBinary(TreeConfig _bTree, string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(configPath + _name + ".btree", System.IO.FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _bTree);
            fs.Close();
        }

        public static TreeConfig ReadBinary(string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(configPath + _name + ".btree", System.IO.FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            TreeConfig bTree = binaryFormatter.Deserialize(fs) as TreeConfig;
            fs.Close();
            return bTree;
        }

        public static void WriteXML(TreeConfig _bTree, string _name)
        {
            XmlSerializer writer = new XmlSerializer(typeof(TreeConfig));
            System.IO.StreamWriter file = new System.IO.StreamWriter(configPath + _name + ".xml");
            writer.Serialize(file, _bTree);
            file.Close();
        }
        public static TreeConfig ReadXML(string _name)
        {
            XmlSerializer reader = new XmlSerializer(typeof(TreeConfig));
            System.IO.StreamReader file = new System.IO.StreamReader(configPath + _name + ".xml");
            TreeConfig btree = reader.Deserialize(file) as TreeConfig;
            file.Close();
            return btree;
        }
    }
}
