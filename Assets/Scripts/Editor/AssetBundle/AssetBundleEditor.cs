using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public class AssetBundleEditor : Editor
{
    [MenuItem("BuildAssetBundle/BuildAssets")]
    static void BuildAssets()
    {
        string tmpOutPath = Application.dataPath + "/../StreamingAssets/";
        if (Directory.Exists(tmpOutPath))
        {
            Directory.Delete(tmpOutPath, true);
        }

        if (Directory.Exists(tmpOutPath) == false)
        {
            Directory.CreateDirectory(tmpOutPath);
        }
        //打包资源
        BuildPipeline.BuildAssetBundles(tmpOutPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

    }

    [MenuItem("Assets/BuildSelectAssetBundleName")]
    static void BuildSelectAssetBundleName()
    {
        EditorUtility.DisplayProgressBar("Clear AssetBundleName", "AssetBundleName", 0f);

        SetAssetBundleName("","");

        EditorUtility.ClearProgressBar();


        UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);

        for (int i = 0; i < arr.Length; ++i)
        {
            string tmpStringFilePath = AssetDatabase.GetAssetPath(arr[i]);

            Debug.Log(tmpStringFilePath);

            AssetImporter tmpAssetImport = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(arr[i]));

            if (tmpAssetImport == null) return;

            tmpAssetImport.assetBundleName = tmpAssetImport.assetPath;
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("BuildAssetBundle/SetAssetBundleName")]
    static void SetAssetBundleName()
    {
        EditorUtility.DisplayProgressBar("SetAssetBundleName", "SetAssetBundleName", 0f);

        SetAssetBundleName("","assetbundle.unity3d");

        EditorUtility.ClearProgressBar();
    }

    [MenuItem("BuildAssetBundle/SetLuaAssetBundleName")]
    static void SetLuaAssetBundleName()
    {
        EditorUtility.DisplayProgressBar("SetAssetBundleName", "SetLuaAssetBundleName", 0f);

        SetAssetBundleName("R/Lua/","lua.unity3d");

        EditorUtility.ClearProgressBar();
    }

    [MenuItem("BuildAssetBundle/ClearAssetBundleName")]
    static void ClearAssetBundleName()
    {
        EditorUtility.DisplayProgressBar("SetAssetBundleName", "SetAssetBundleName", 0f);

        SetAssetBundleName("","");

        EditorUtility.ClearProgressBar();
    }

    static void SetAssetBundleName(string dir, string assetbundleName)
    {
        string tmpStringPathR = Application.dataPath + "/" + dir;

        //找到目录里所有资源 修改AssetbundleName
        string[] tmpFileArray = Directory.GetFiles(tmpStringPathR, "*.*", SearchOption.AllDirectories);

        for (int i = 0; i < tmpFileArray.Length; i++)
        {
            string tmpStringFilePath = tmpFileArray[i];

            if (tmpStringFilePath.EndsWith(".cs")
                || tmpStringFilePath.EndsWith(".meta"))
            {
                continue;
            }

            tmpStringFilePath = tmpStringFilePath.Replace("\\", "/").Replace(Application.dataPath + "/", "");
            tmpStringFilePath = "Assets/" + tmpStringFilePath;
            Debug.Log(tmpStringFilePath);
            AssetImporter tmpAssetImport = AssetImporter.GetAtPath(tmpStringFilePath);

            if (tmpAssetImport == null) continue;

            if (tmpStringFilePath.EndsWith(".prefab")
                || tmpStringFilePath.EndsWith(".anim")
                || tmpStringFilePath.EndsWith(".txt"))
            {
                UnityEngine.Object go = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(tmpAssetImport.assetPath);
                if(go == null)
                {
                    tmpAssetImport.assetBundleName = "";
                    continue;
                }
                if ( (tmpStringFilePath.Contains("NGUI/") && go.GetType()==typeof(GameObject) && ((GameObject)go).GetComponent<UIAtlas>())
                     ||tmpStringFilePath.Contains("R/")
                    )
                {
                    tmpAssetImport.assetBundleName = assetbundleName;
                }
                else
                {
                    tmpAssetImport.assetBundleName = "";
                }
            }
            else
            {
                tmpAssetImport.assetBundleName = "";
            }
            EditorUtility.DisplayProgressBar("Set AssetBundleName", "Setting:" + tmpStringFilePath, i / (float)tmpFileArray.Length);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

  

    [MenuItem("BuildAssetBundle/SetEditorMode")]
    static void SetEditorMode()
    {
        PlayerPrefs.SetInt("assetmode", 0);
    }
    [MenuItem("BuildAssetBundle/SetAssetBundleMode")]
    static void SetAssetBundleMode()
    {
        PlayerPrefs.SetInt("assetmode", 1);
    }
}
