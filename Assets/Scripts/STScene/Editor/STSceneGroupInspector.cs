using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof (STSceneGroup))]
public class STSceneGroupInspector : Editor
{
    protected STSceneGroup mTarget;
    private List<Type> mEntityTypeList = new List<Type>();
    private void Awake()
    {
        mTarget = target as STSceneGroup;
        if (string.IsNullOrEmpty(mTarget.name))
        {
            mTarget.name = "STGroup";
        }
        for (int i = 0; i < 4; i++)
        {
            Assembly assembly = null;
            try
            {
                switch (i)
                {
                    case 0:
                        assembly = Assembly.Load("Assembly-CSharp");
                        break;
                    case 1:
                        assembly = Assembly.Load("Assembly-CSharp-firstpass");
                        break;
                    case 2:
                        assembly = Assembly.Load("Assembly-UnityScript");
                        break;
                    case 3:
                        assembly = Assembly.Load("Assembly-UnityScript-firstpass");
                        break;
                }
            }
            catch (Exception)
            {
                assembly = null;
            }
            if (assembly != null)
            {
                Type[] types = assembly.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (!types[j].IsAbstract)
                    {
                        if (types[j] == typeof(STSceneEntity) || types[j].IsSubclassOf(typeof(STSceneEntity)))
                        {
                            mEntityTypeList.Add(types[j]);
                        }
                       
                    }
                }
            }
        }
    }
   
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
     
        if (mTarget == null)
        {
            return;
        }
        mTarget.name = EditorGUILayout.TextField("GroupName", mTarget.name);
        GUILayout.BeginHorizontal();
        mTarget.SetAttribute();

        for (int i = 0; i < mEntityTypeList.Count; ++i)
        {
            Type type = mEntityTypeList[i];
            if (GUILayout.Button("Add "+ type.Name))
            {
                AddSTComponentToGroup(mTarget, type);
            }
        }
        
        for (int i = mTarget.components.Count - 1; i  >=0; i --)
        {
            if(mTarget.components[i] == null)
            {
                mTarget.components.RemoveAt(i);
            }
        }
        GUILayout.EndHorizontal();
        mTarget.UpdateAttribute();
    }


    public static T AddSTComponentToGroup<T>(STSceneGroup group) where T : STComponent
    {
        return AddSTComponentToGroup(group,typeof(T)) as T;
    }

    public static STComponent AddSTComponentToGroup(STSceneGroup group, Type type)
    {
        GameObject go = new GameObject(type.ToString());

        go.transform.SetParent(group.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        STComponent component = go.AddComponent(type) as STComponent;

        group.AddSTComponent(component);

        return component;
    }

    [InitializeOnLoadMethod]
    static void Init()
    {
        //SceneView.onSceneGUIDelegate += OnSceneGUI;

        //EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }
    /*
       private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
       {
           Event e = Event.current;
           if (e.type == EventType.DragExited)
           {
               Debug.Log(e.type);
               DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

               DragAndDrop.AcceptDrag();
               for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
               {
                   UnityEngine.Object handleObj = DragAndDrop.objectReferences[i];
                   if (handleObj != null)
                   {

                       string path = UnityEditor.AssetDatabase.GetAssetPath(handleObj);
                       GameObject go = handleObj as GameObject;

                       Debug.Log(path);
                   }
               }

           }

       }
       */
    /*
    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    UnityEngine.Object handleObj = DragAndDrop.objectReferences[i];
                    if (handleObj != null)
                    {
                       
                       string path = UnityEditor.AssetDatabase.GetAssetPath(handleObj);
                        Debug.Log(path);
                    }
                }
            }
            
        }
    }
    */

    static void OnHierarchyChanged()
    {
        STSceneGroup[] groups = FindObjectsOfType<STSceneGroup>();

        for(int i = 0; i <groups.Length; ++i)
        {
            for(int j = 0; j < groups[i].transform.childCount; ++j)
            {
                Transform child = groups[i].transform.GetChild(j);
                STComponent component = child.GetComponent<STComponent>();
                if(component==null)
                {
                    var targetPrefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject) as GameObject;
                    string path = UnityEditor.AssetDatabase.GetAssetPath(targetPrefab);
                    STSceneEntity entity = AddSTComponentToGroup<STSceneEntity>(groups[i]);
                    child.SetParent(entity.transform);
                    entity.path = path;
                }
            }
        }

    }
}