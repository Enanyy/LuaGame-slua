using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(STScene))]
public class STSceneInspector:STSceneGroupInspector
{
    private void Awake()
    {
        mTarget = target as STScene;
        if (string.IsNullOrEmpty(mTarget.name))
        {
            mTarget.name = "STscene";
        }
    }
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();

        var scene = mTarget as STScene;

        scene.groupName = EditorGUILayout.TextField("SceneName", scene.groupName);
        scene.startPoint = EditorGUILayout.Vector2Field("StartPoint", scene.startPoint);
        scene.width = EditorGUILayout.IntField("Width", scene.width);
        scene.height = EditorGUILayout.IntField("Height", scene.height);

        GUILayout.BeginHorizontal();
       
        if (GUILayout.Button("Add Group"))
        {
            AddSTComponentToGroup<STSceneGroup>(scene);

        }
        bool existCamera = false;

        for(int i = scene.components.Count-1; i >=0;--i)
        {
            if(scene.components[i] == null)
            {
                scene.components.RemoveAt(i);continue;
            }
            if(scene.components[i].GetType() == typeof(STSceneCamera))
            {
                existCamera = true;break;
            }
        }
        if (existCamera == false)
        {
            if (GUILayout.Button("Add Camera"))
            {
                AddSTComponentToGroup<STSceneCamera>(scene);  
            }
        }
        
        GUILayout.EndHorizontal();
        if (scene.transform.childCount == 0)
        {
            if (GUILayout.Button("Load Xml"))
            {
                LoadXml();
            }
        }

        if (scene.transform.childCount > 0)
        {
            if (GUILayout.Button("Clear"))
            {
                ClearEntity();
            }
            if (GUILayout.Button("Save Xml"))
            {
                SaveXml();
            }
        }

    }

    private void ClearEntity()
    {
        for (int i = mTarget.transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(mTarget.transform.GetChild(i).gameObject);
        }
        mTarget.components.Clear();
    }

    private void SaveXml()
    {
        XmlDocument doc = new XmlDocument();
        XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        doc.InsertBefore(dec, doc.DocumentElement);

        doc.AppendChild(mTarget.ToXml(doc));

        MemoryStream ms = new MemoryStream();
        XmlTextWriter xw = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
        xw.Formatting = Formatting.Indented;
        doc.Save(xw);

        ms = (MemoryStream)xw.BaseStream;
        byte[] bytes = ms.ToArray();
        string xml = System.Text.Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

        string path = EditorUtility.SaveFilePanel("导出场景配置文件", Application.dataPath + "/AssetBundle/config/scene/", "STScene", "txt");
        EditorUtility.DisplayProgressBar("请稍候", "正在导出场景配置文件", 0.1f);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, xml, System.Text.Encoding.UTF8);

           // ClearEntity();

        }
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("STScene/Open")]
    static void LoadXml()
    {
        string path = EditorUtility.OpenFilePanel("Select a scene config", Application.dataPath + "/AssetBundle/config/scene/", "txt");

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        Debug.Log(path);


        string text = File.ReadAllText(path);


        var scene = GameObject.FindObjectOfType<STScene>();
        if (scene == null)
        {
            GameObject go = new GameObject(typeof(STScene).ToString());
            scene = go.AddComponent<STScene>();
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }

        for (int i = scene.transform.childCount - 1; i >= 0; --i)
        {
            GameObject.DestroyImmediate(scene.transform.GetChild(i).gameObject);
        }
        scene.components.Clear();

        scene.LoadXml(text,(count,finishCount)=> { Debug.Log("count = " + count + ",finishCount=" + finishCount); });
    }

    [MenuItem("STScene/Create")]
    static void CreateSTScene()
    {
        var scene = GameObject.FindObjectOfType<STScene>();
        if (scene == null)
        {
            GameObject go = new GameObject(typeof(STScene).ToString());
            scene = go.AddComponent<STScene>();
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
           

        }
    }
}

