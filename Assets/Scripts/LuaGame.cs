using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SLua;
using UnityEngine.SceneManagement;

public partial class LuaGame : MonoBehaviour
{
    LuaSvr l;
    int progress = 0;
    string sceneName = "Battle";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); 
    }

    // Use this for initialization
    void Start()
    {
        if (LuaFile.assetmode == 1)
        {
            var luabundle = AssetBundle.LoadFromFile(Application.dataPath + "/../StreamingAssets/lua.unity3d");
            LuaFile.AddLuaBundle(luabundle);
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

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Example");
       

    }
#if UNITY_EDITOR
    void OnGUI()
    {
        if (progress != 100)
            GUI.Label(new Rect(0, 0, 100, 50), string.Format("Loading {0}%", progress));

        if(GUI.Button(new Rect(20,20,100,40),sceneName))
        {
           
            SceneManager.LoadScene(sceneName);
        }

    }
#endif

    byte[] OnLoad(string fn, ref string absoluteFn)
    {
        string path = LuaFile.FindFile(fn.Replace('.', '/'));

        byte[] bytes = LuaFile.ReadBytes(path);

        return bytes;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
    }

}
