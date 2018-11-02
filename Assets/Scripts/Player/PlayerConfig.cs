using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public static class PlayerConfig
{
    public static string path = Application.dataPath + "/R/Config/Player/";
    public static  byte[] ReadXML(string name)
    {
        string fullPath = string.Format("{0}{1}.xml", path , name );

        if (File.Exists(fullPath))
        {
            return File.ReadAllBytes(fullPath);
        }
        return null;
    }

    public static byte[] ReadBinary(string name)
    {
        string fullPath = string.Format("{0}{1}.btree", path, name);

        if (File.Exists(fullPath))
        {
            return File.ReadAllBytes(fullPath);
        }
        return null;
    }
}

