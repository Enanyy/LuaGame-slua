using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using System.Xml;
#endif
using System.Security;
using Mono.Xml;

public class STScene : STSceneGroup
{
    [HideInInspector] public Vector2 startPoint;
    [HideInInspector] public int width =100;
    [HideInInspector] public int height= 100;

    private Action<int,int> mCallback;

    protected override void OnLoadFinish()
    {
        int count = 0;
        int finishCount = 0;

        CheckFinish(out count, out finishCount);

        if (mCallback != null)
        {
            mCallback(count, finishCount);
        }
    }

    public override void ParseXml(SecurityElement node)
    {
        if(node == null)
        {
            return;
        }
        base.ParseXml(node);    

        if(IsTypeOrSubClass(node.Tag,typeof(STScene)))
        {
            startPoint = node.Attribute("startPoint").ToVector2Ex();
            width = node.Attribute("width").ToInt32Ex();
            height = node.Attribute("height").ToInt32Ex();
        }
    }
#if UNITY_EDITOR
    public override XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes = null)
    {
        if(attributes== null)
        {
            attributes = new Dictionary<string, string>();
        }

        attributes.Add("startPoint", startPoint.ToString());
        attributes.Add("width", width.ToString());
        attributes.Add("height", height.ToString());

        return base.ToXml(parent, attributes);
    }
#endif
    public void LoadXml(string text, Action<int,int> callback= null)
    {
        if(string.IsNullOrEmpty(text))
        {
            return;

        }
        mCallback = callback;
        SecurityParser sp = new SecurityParser();

        sp.LoadXml(text);


        SecurityElement se = sp.ToXml();

        ParseXml(se);

        SetAttribute();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Color defualtColor = Gizmos.color;
        Gizmos.color = Color.green;

        Vector3 leftBottom = new Vector3(startPoint.x, 0, startPoint.y);
        Vector3 rightBottom = leftBottom + Vector3.right * width;
        Vector3 leftTop = leftBottom + Vector3.forward * height;
        Vector3 rightTop = rightBottom + Vector3.forward * height;

        Gizmos.DrawLine(leftBottom, rightBottom);
        Gizmos.DrawLine(leftBottom, leftTop);
        Gizmos.DrawLine(leftTop, rightTop);
        Gizmos.DrawLine(rightBottom, rightTop);


        Gizmos.color = defualtColor;
    }

#endif


}
