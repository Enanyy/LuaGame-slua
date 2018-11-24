using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

#if UNITY_EDITOR
using System.Xml;
#endif
public class STScenePoint : STSceneEntity
{
    public int index;
    public override void ParseXml(SecurityElement node)
    {
        if (node == null)
        {
            return;
        }
        base.ParseXml(node);
        if (IsTypeOrSubClass(node.Tag, typeof(STScenePoint)))
        {
            index = node.Attribute("index").ToInt32Ex();
        }
    }

   
#if UNITY_EDITOR
    public override XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes = null)
    {
        if (attributes == null)
        {
            attributes = new Dictionary<string, string>();
        }
        attributes.Add("index", index.ToString());
      
      
        return base.ToXml(parent, attributes);
    }
#endif
   
}

