using System;
using System.Collections.Generic;
using System.Security;
#if UNITY_EDITOR
using System.Xml;
#endif
using UnityEngine;
public class STSceneCamera : STComponent
{
    [HideInInspector] public float fieldOfView = 60;
    [HideInInspector] public float nearClipPlane = 0.3f;
    [HideInInspector] public float farClipPlane = 600f;
    [HideInInspector] public float depth = 0;


    public override void ParseXml(SecurityElement node)
    {
        if (node == null)
        {
            return;
        }
        base.ParseXml(node);
        if (IsTypeOrSubClass(node.Tag, typeof(STSceneCamera)))
        {
          
            fieldOfView = node.Attribute("fieldOfView").ToFloatEx();
            nearClipPlane = node.Attribute("nearClipPlane").ToFloatEx();
            farClipPlane = node.Attribute("farClipPlane").ToFloatEx();
            depth = node.Attribute("depth").ToFloatEx();
        }
    }

    public override void SetAttribute()
    {
        if (Application.isPlaying == false)
        {
            Camera camera = GetComponent<Camera>();
            if (camera == null) camera = gameObject.AddComponent<Camera>();
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = nearClipPlane;
            camera.farClipPlane = farClipPlane;
            camera.depth = depth;
        }
    }

#if UNITY_EDITOR
    public override XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes)
    {
        if (attributes == null)
        {
            attributes = new Dictionary<string, string>();
        }
      
        attributes.Add("fieldOfView", fieldOfView.ToString());
        attributes.Add("nearClipPlane", nearClipPlane.ToString());
        attributes.Add("farClipPlane", farClipPlane.ToString());
        attributes.Add("depth", depth.ToString());

        return base.ToXml(parent,attributes);
    }

   
#endif

}

