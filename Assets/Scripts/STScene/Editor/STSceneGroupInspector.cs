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


    public T AddSTComponentToGroup<T>(STSceneGroup group) where T : STComponent
    {
        return AddSTComponentToGroup(group,typeof(T)) as T;
    }

    public STComponent AddSTComponentToGroup(STSceneGroup group, Type type)
    {
        GameObject go = new GameObject(type.ToString());

        go.transform.SetParent(mTarget.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        STComponent component = go.AddComponent(type) as STComponent;

        group.AddSTComponent(component);

        return component;
    }
}