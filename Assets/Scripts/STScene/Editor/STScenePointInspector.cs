using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(STScenePoint))]
public class STScenePointInspector : STSceneEntityInspector
{
   

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
       


        mTarget.UpdateAttribute();
    }
}

