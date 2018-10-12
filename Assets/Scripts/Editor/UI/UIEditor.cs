using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

using System.IO;
using System.Text;

/*
 * 
 * 根据UI的prefab中的物件命名自动生成初始化代码
 * 命名规则 name@type.variableName
 * 解释：物体名称（可有可无）@要获取该物体上的对象类型.自动生成代码的变量名
 * 如：Icon@sprite.mItemIcon
 * 生成初始化代码 UISprite mItemIcon = transform.Find("XXXX/Icon").GetComponent<UISprite>();
 * XXXX是该物体在prefab中的路径
*/

public class Variable
{
    string name = string.Empty;
    string type = string.Empty;
    string path = string.Empty;

    public Variable(string varName, string varType, string varPath)
    {
        name = varName;
        type = varType;
        path = varPath;
    }
    public string Name { get { return name; } }
    public string Type { get { return type; } }
    public string Path { get { return path; } }

}

public class UIEditor
{



    [MenuItem("Assets/Create UI base", true)]
    static bool IsPrefab()
    {
        return (Selection.activeObject != null && Selection.activeObject.GetType() == typeof(GameObject));
    }

    [MenuItem("Assets/Create UI base")]
    static void CreateCSharpFile()
    {
        GameObject ui = Selection.activeGameObject;
        if (!ui)
        {
            Debug.Log("请选择一个UI Prefab");
            return;
        }

        Dictionary<string, Variable> variableDir = ParseUIPrefab(ui);

        if (variableDir == null || variableDir.Count == 0)
            return;

        string fullpath = GetCSharpFileName(ui);
        if (File.Exists(fullpath))
            File.Delete(fullpath);

        try
        {
            FileStream fs = new FileStream(fullpath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(WriteString(ui.name, variableDir));
            sw.Close();
        }
        catch (System.Exception e)
        {
            throw e;
        }

        Debug.Log("Done!!");
        AssetDatabase.Refresh();
    }

    static string WriteString(string className, Dictionary<string, Variable> variableDir)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("--[[\n本文件由UIEditor工具自动生成，DO NOT EDIT!!!\n--]]\n\n");
        builder.Append(string.Format("function {0}Base(self)\n",className));
        
        foreach (var v in variableDir)
        {
           builder.Append(string.Format("\tself.{0} = self.transform:Find(\"{1}\"):GetComponent({2})\n", v.Key, v.Value.Path, v.Value.Type));
        }

        builder.Append("end");

        return builder.ToString();
    }

    static Dictionary<string, Variable> ParseUIPrefab(GameObject ui)
    {
        Dictionary<string, Variable> variableDir = new Dictionary<string, Variable>();

        if (ui)
        {
            Transform[] childs = ui.GetComponentsInChildren<Transform>(true);

            foreach (var t in childs)
            {
                Transform child = t;
                if (child == ui.transform)
                    continue;

                string name = child.name;
                if (!name.Contains("@"))
                    continue;

                int index = name.IndexOf('@');
                if (index >= name.Length - 1)
                    continue;

                string variableNameAndType = name.Substring(index + 1);

                string variableName, type, path;

                if (variableNameAndType.Contains("."))
                {
                    string[] nameAndTypes = variableNameAndType.Split('.');
                    if (nameAndTypes.Length != 2)
                    {
                        variableDir.Clear();
                        Debug.LogError(string.Format("命名错误：{0}", name));
                        return variableDir;
                    }

                    type = nameAndTypes[0];
                    variableName = nameAndTypes[1];

                }
                else
                {
                    type = "transform";
                    variableName = variableNameAndType;
                }

                System.Type variableType = GetType(type);
                if (variableType == null)
                {
                    variableDir.Clear();
                    Debug.LogError(string.Format("命名错误,没定义该类型：{0}", name));
                    return variableDir;
                }

                if (!child.GetComponent(variableType))
                {
                    variableDir.Clear();
                    Debug.LogError(string.Format("给定的物体{0}没有{1}类型的组件", name, variableType));
                    return variableDir;
                }

                path = name;

                while (child.parent && child.parent != ui.transform)
                {
                    path = string.Format("{0}/{1}", child.parent.name, path);
                    child = child.parent;
                }

                if (!ui.transform.Find(path))
                {
                    Debug.LogError(string.Format("根据路径 path = {0}未能找到物体！", path));
                    variableDir.Clear();

                    return variableDir;
                }

                if (!ui.transform.Find(path).GetComponent(variableType))
                {
                    Debug.LogError(string.Format("根据路径 path = {0}未能找到物体上的{1}组件！", path, variableType.ToString()));
                    variableDir.Clear();
                    return variableDir;
                }

                if (variableDir.ContainsKey(variableName))
                {
                    Debug.LogError(string.Format("重复变量名：{0}, path = {1}", variableName, path));
                    variableDir.Clear();
                    return variableDir;
                }
                else
                {
                    if (!string.IsNullOrEmpty(variableName) && !string.IsNullOrEmpty(variableType.ToString()) && !string.IsNullOrEmpty(path))
                    {
                        variableDir.Add(variableName, new Variable(variableName, variableType.ToString(), path));
                    }
                    else
                    {
                        variableDir.Clear();
                        Debug.LogError(string.Format("命名错误：{0}", name));
                        return variableDir;
                    }
                }
            }
        }

        return variableDir;
    }


    static System.Type GetType(string type)
    {
        switch (type.ToLower())
        {
            case "gameobject": return typeof(Transform);
            case "transform": return typeof(Transform);
            case "camera": return typeof(Camera);
            case "panel": return typeof(UIPanel);
            case "boxcollider": return typeof(BoxCollider);
            case "label": return typeof(UILabel);
            case "button": return typeof(UIButton);
            case "sprite": return typeof(UISprite);
            case "texture": return typeof(UITexture);
            case "grid": return typeof(UIGrid);
            case "scrollbar": return typeof(UIScrollBar);
            case "toggle": return typeof(UIToggle);
            case "scrollview": return typeof(UIScrollView);
            case "table": return typeof(UITable);
            case "input": return typeof(UIInput);
            case "wrapcontent": return typeof(UIWrapContent);
            case "tweenrotation": return typeof(TweenRotation);
            case "tweenalpha": return typeof(TweenAlpha);
            case "tweenposition": return typeof(TweenPosition);
            case "tweenscale": return typeof(TweenScale);
            case "tweencolor":return typeof(TweenColor);
                
            default:
                return null;
        }

    }

    static string GetCSharpFileName(GameObject ui)
    {
        string path = Application.dataPath + "/R/Lua/UI/" + ui.name + "/Base/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = path + ui.name + "Base.txt";
        return path;
    }
}
