
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BTree.Editor
{
    public class BTEditorSerialization
    {
        public static string saveConfigPath = "Assets/Editor/BTreeEditor/Config/";
        public static string exportConfigPath = "Assets/R/Config/Player/";

        public static string mSuffix = ".btreeEditor";
        public static void WriteBinary(BTEditorConfig _bTree, string _name)
        {
            FileStream fs = new FileStream(saveConfigPath + _name + mSuffix, FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _bTree);
            fs.Close();
        }

        public static BTEditorConfig ReadBinary(string _name)
        {
            FileStream fs = new FileStream(saveConfigPath + _name + mSuffix, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            BTEditorConfig bTree = binaryFormatter.Deserialize(fs) as BTEditorConfig;
            fs.Close();
            return bTree;
        }

        public static void WriteXML(BTEditorConfig _bTree, string _name)
        {
            WirteXMLAtPath(_bTree, saveConfigPath + _name + ".xml");
        }
        public static void WirteXMLAtPath(BTEditorConfig _bTree, string _path)
        {
            XmlSerializer writer = new XmlSerializer(typeof(BTEditorConfig));
            StreamWriter file = new StreamWriter(_path);
            writer.Serialize(file, _bTree);
            file.Close();
        }
        public static BTEditorConfig ReadXML(string _name)
        {
            return ReadXMLAtPath(saveConfigPath + _name + ".xml");
        }
        public static BTEditorConfig ReadXMLAtPath(string _path)
        {
            XmlSerializer reader = new XmlSerializer(typeof(BTEditorConfig));
            StreamReader file = new StreamReader(_path);
            BTEditorConfig btree = reader.Deserialize(file) as BTEditorConfig;
            file.Close();
            return btree;
        }

        public static void ExportBinary(TreeConfig _bTree, string _name)
        {
            FileStream fs = new FileStream(exportConfigPath + _name + ".btree", FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _bTree);
            fs.Close();
        }
        public static void ExportXML(TreeConfig _bTree, string _name)
        {
            XmlSerializer writer = new XmlSerializer(typeof(TreeConfig));
            StreamWriter file = new StreamWriter(exportConfigPath + _name + ".xml");
            writer.Serialize(file, _bTree);
            file.Close();
        }
    }
}
