using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuaFile
{
    public static string luaDir = Application.dataPath + "/Scripts/Lua";                //lua逻辑代码目录

    protected static List<string> searchPaths = new List<string>();

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

    //格式: 路径/?.lua
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

            sb.Append("?.txt");
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
        if (fileName == string.Empty)
        {
            return string.Empty;
        }

        if (Path.IsPathRooted(fileName))
        {
            if (!fileName.EndsWith(".txt"))
            {
                fileName += ".txt";
            }

            return fileName;
        }

        if (fileName.EndsWith(".txt"))
        {
            fileName = fileName.Substring(0, fileName.Length - 4);
        }

        string fullPath = null;

        for (int i = 0; i < searchPaths.Count; i++)
        {
            fullPath = searchPaths[i].Replace("?", fileName);

            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        return null;
    }
}

