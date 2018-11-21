using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(STSceneCamera))]
public class STSceneCameraInspector : Editor
{
    private STSceneCamera mTarget;
    private void Awake()
    {
        mTarget = target as STSceneCamera;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Camera camera = mTarget.GetComponent<Camera>();
        if (camera == null) camera = mTarget.gameObject.AddComponent<Camera>();
        mTarget.farClipPlane = camera.farClipPlane;
        mTarget.nearClipPlane = camera.nearClipPlane;
        mTarget.depth = camera.depth;
        mTarget.fieldOfView = camera.fieldOfView;

        mTarget.UpdateAttribute();
    }

}

