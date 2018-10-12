using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SLua;
public partial class LuaGame : MonoBehaviour
{
    LuaSvr l;
    int progress = 0;
    AssetBundle luabundle;

    private int mAssetmode = -1;
    public int assetmode
    {
        get
        {
            if(mAssetmode == -1)
            {
                if(PlayerPrefs.HasKey("assetmode"))
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
    // Use this for initialization
    void Start()
    {
        if (assetmode == 1)
        {
            luabundle = AssetBundle.LoadFromFile(Application.dataPath + "/../StreamingAssets/lua.unity3d");
            LuaFile.AddSearchPath(luabundle.GetAllAssetNames());
        }
        else
        {
            LuaFile.AddSearchPath(Application.dataPath + "/R/Lua",true); //lua逻辑代码目录

        }
        Application.logMessageReceived += OnLog;
        l = new LuaSvr();

        LuaSvr.mainState.loaderDelegate = OnLoad;

        l.init(OnTick, OnComplete, LuaSvrFlag.LSF_BASIC | LuaSvrFlag.LSF_EXTLIB);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnLog(string cond, string trace, LogType lt)
    {
        //Debug.Log(trace);
    }

    void OnTick(int p)
    {
        progress = p;
    }

    void OnComplete()
    {
        l.start("main");
    }

    void OnGUI()
    {
        if (progress != 100)
            GUI.Label(new Rect(0, 0, 100, 50), string.Format("Loading {0}%", progress));
    }

    byte[] OnLoad(string fn, ref string absoluteFn)
    {
        string path = LuaFile.FindFile(fn.Replace('.', '/'));

        if (assetmode == 1)
        {
            if(luabundle!=null)
            {
                TextAsset asset = luabundle.LoadAsset<TextAsset>(path);
                if(asset)
                {
                    return asset.bytes;
                }
            }
        }
        else
        {
            if (System.IO.File.Exists(path))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                return bytes;
            }
        }

        return null;
    }

}
