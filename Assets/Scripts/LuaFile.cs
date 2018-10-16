using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuaFile
{
    private const string extension = ".txt";

    private static int mAssetmode = -1;
    public static int assetmode
    {
        get
        {
            if (mAssetmode == -1)
            {
                if (PlayerPrefs.HasKey("assetmode"))
                {
                    mAssetmode = PlayerPrefs.GetInt("assetmode");
                }
                else
                {
                    mAssetmode = 0;
                }
            }
            return mAssetmode;
        }
    }

    private static List<string> searchPaths = new List<string>();
    private static List<string> assetNames = new List<string>();

    /// <summary>
    /// lua脚本bundle
    /// </summary>
    private static AssetBundle luabundle;

    public static void AddLuaBundle(AssetBundle bundle )
    {
        luabundle = bundle;
        if(luabundle)
        {
            AddSearchPath(bundle.GetAllAssetNames());
        }
    }

    private static void AddSearchPath(string[] assetnames)
    {
        assetNames.AddRange(assetnames);

        for (int i = 0; i < assetNames.Count; ++i)
        {
            string path = assetNames[i].Substring(0, assetNames[i].LastIndexOf('/') + 1);
            path += "?" + extension;
            if (searchPaths.Contains(path) == false)
            {
                searchPaths.Add(path);
            }
        }
    }
    /// <summary>
    /// 添加搜索目录，包括所有子目录
    /// </summary>
    /// <param name="path"></param>
    public static void AddSearchPath(string path,bool includeSubdirectory)
    {
        AddSearchPath(path);
        if (includeSubdirectory)
        {
            string[] paths = Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; ++i)
            {
                AddSearchPath(paths[i]);
            }
        }
    }

    public static bool AddSearchPath(string path)
    {
        path = ToPackagePath(path);

        int index = searchPaths.IndexOf(path);

        if (index >= 0)
        {
            return false;
        }
    
        searchPaths.Add(path);

        return true;
    }

    static string  ToPackagePath(string path)
    {
        using (CString.Block())
        {
            CString sb = CString.Alloc(256);
            sb.Append(path);
            sb.Replace('\\', '/');

            if (sb.Length > 0 && sb[sb.Length - 1] != '/')
            {
                sb.Append('/');
            }

            sb.Append("?");
            sb.Append(extension);
            return sb.ToString();
        }
    }

    public static bool RemoveSearchPath(string path)
    {
        int index = searchPaths.IndexOf(path);

        if (index >= 0)
        {
            searchPaths.RemoveAt(index);
            return true;
        }

        return false;
    }


    public static string FindFile(string fileName)
    {
        if ( string.IsNullOrEmpty(fileName))
        {
            return string.Empty;
        }

        if (Path.IsPathRooted(fileName))
        {
            if (!fileName.EndsWith(extension))
            {
                fileName += extension;
            }

            return fileName;
        }

        if (fileName.EndsWith(extension))
        {
            fileName = fileName.Substring(0, fileName.Length - 4);
        }

        string fullPath = null;

        for (int i = 0; i < searchPaths.Count; i++)
        {
            fullPath = searchPaths[i].Replace("?", fileName);
            if(assetmode == 1&& assetNames.Contains(fullPath))
            {
                return fullPath;
            }
            else if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        return null;
    }

    public static byte[] ReadBytes(string file)
    {
        if (assetmode == 1)
        {
            if (luabundle != null)
            {
                TextAsset asset = luabundle.LoadAsset<TextAsset>(file);
                if (asset)
                {
                    return asset.bytes;
                }
            }
        }
        else
        {
            if (File.Exists(file))
            {
                byte[] bytes = File.ReadAllBytes(file);
                return bytes;
            }
        }
        return null;
    }
   
    public static string ReadString(string file)
    {
        if (assetmode == 1)
        {
            if (luabundle != null)
            {
                TextAsset asset = luabundle.LoadAsset<TextAsset>(file);
                if (asset)
                {
                    return asset.text;
                }
            }
        }
        else
        {
            if (File.Exists(file))
            {
                var text = File.ReadAllText(file);
                return text;
            }
        }
        return null;
    }
    public static void Clear()
    {
        searchPaths.Clear();
        assetNames.Clear();
        luabundle.Unload(true);
        luabundle = null;
    }
}

