using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(STSceneEntity))]
public class STSceneEntityInspector : Editor
{
    protected STSceneEntity mTarget;

    void OnEnable()
    {
        mTarget = target as STSceneEntity;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (mTarget == null)
        {
            return;
        }

        mTarget.localPosition = EditorGUILayout.Vector3Field("LocalPosition", mTarget.localPosition);
        mTarget.localRotation = EditorGUILayout.Vector3Field("LocalRotation", mTarget.localRotation);
        mTarget.localScale = EditorGUILayout.Vector3Field("LocalScale", mTarget.localScale);
        if (mTarget.mGo)
        {
            mTarget.mGo.transform.localPosition = mTarget.localPosition;
            mTarget.mGo.transform.localRotation = Quaternion.Euler(mTarget.localRotation);
            mTarget.mGo.transform.localScale = mTarget.localScale;
        }

        GUILayout.BeginHorizontal();
        mTarget.path = EditorGUILayout.TextField("Asset Path", mTarget.path, GUILayout.MinWidth(20f));
        if (GUILayout.Button("Edit", GUILayout.Width(40f)))
        {
            string path = EditorUtility.OpenFilePanel("Select a ground", Application.dataPath + "/AssetBundle/", "prefab");

            path = path.Substring(path.IndexOf("Assets/"));
            mTarget.path = path;
            if (mTarget.mGo)
            {
                DestroyImmediate(mTarget.mGo);
            }
           
        }
        if (string.IsNullOrEmpty(mTarget.path) == false)
        {
            string assetName = System.IO.Path.GetFileNameWithoutExtension(mTarget.path);
            mTarget.name = mTarget.GetType().Name + "+" + assetName;
            mTarget.SetAttribute();
        }

        if (mTarget.mGo != null)
        {
            if (GUILayout.Button("Clear", GUILayout.Width(50f)))
            {
                if (mTarget.mGo)
                {
                    DestroyImmediate(mTarget.mGo);
                }
                mTarget.path = null;
            }
        }

        GUILayout.EndHorizontal();
        mTarget.UpdateAttribute();
    }
}

