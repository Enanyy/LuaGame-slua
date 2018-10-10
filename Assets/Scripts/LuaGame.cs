using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SLua;
public partial class LuaGame : MonoBehaviour
{
    LuaSvr l;
    int progress = 0;
    // Use this for initialization
    void Start()
    {
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
        Debug.Log(trace);
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
        string path = string.Format("Assets/Scripts/Lua/{0}.txt", fn.Replace('.','/'));
        TextAsset asset=  UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);

        if (asset!=null)
        {
            return asset.bytes;
        }

        return null;
    }

}
