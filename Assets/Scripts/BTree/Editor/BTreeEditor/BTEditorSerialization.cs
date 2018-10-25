
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BTree.Editor
{
    public class BTEditorSerialization
    {
        public static string mConfigPath = "Assets/Editor/BTreeEditor/Config/";
        public static string mSuffix = ".btreeEditor";
        public static void WriteBinary(BTEditorConfig _bTree, string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(mConfigPath + _name + mSuffix, System.IO.FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _bTree);
            fs.Close();
        }

        public static BTEditorConfig ReadBinary(string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(mConfigPath + _name + mSuffix, System.IO.FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            BTEditorConfig bTree = binaryFormatter.Deserialize(fs) as BTEditorConfig;
            fs.Close();
            return bTree;
        }

        public static void WriteXML(BTEditorConfig _bTree, string _name)
        {
            WirteXMLAtPath(_bTree, mConfigPath + _name + ".xml");
        }
        public static void WirteXMLAtPath(BTEditorConfig _bTree, string _path)
        {
            XmlSerializer writer = new XmlSerializer(typeof(BTEditorConfig));
            System.IO.StreamWriter file = new System.IO.StreamWriter(_path);
            writer.Serialize(file, _bTree);
            file.Close();
        }
        public static BTEditorConfig ReadXML(string _name)
        {
            return ReadXMLAtPath(mConfigPath + _name + ".xml");
        }
        public static BTEditorConfig ReadXMLAtPath(string _path)
        {
            XmlSerializer reader = new XmlSerializer(typeof(BTEditorConfig));
            System.IO.StreamReader file = new System.IO.StreamReader(_path);
            BTEditorConfig btree = reader.Deserialize(file) as BTEditorConfig;
            file.Close();
            return btree;
        }
    }
}
