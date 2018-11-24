using System;
using System.Collections.Generic;

using System.Security;
#if UNITY_EDITOR
using System.Xml;
#endif
using UnityEngine;


public abstract class STComponent:MonoBehaviour
{
    public virtual void SetAttribute() { }

    public virtual void CheckFinish(out int count,out int finishCount)
    {
        finishCount = 1;
        count = 1;
    }

    protected virtual void OnLoadFinish()
    {
        if(transform.parent!=null)
        {
            STComponent parent = transform.parent.GetComponent<STComponent>();
            if(parent)
            { 
                parent.OnLoadFinish();
            }
        }
    }

#if UNITY_EDITOR
    public virtual XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes = null)
    {
        if(attributes==null)
        {
            attributes = new Dictionary<string, string>();
        }

        attributes.Add("position", transform.position.ToString());
        attributes.Add("rotation", transform.rotation.eulerAngles.ToString());
        attributes.Add("scale", transform.localScale.ToString());


        return CreateXmlNode(parent, GetType().ToString(), attributes);
    }
   

    public static XmlElement CreateXmlNode(XmlNode parent, string tag, Dictionary<string, string> attributes)
    {
        XmlDocument doc;
        if (parent.ParentNode == null)
        {
            doc = (XmlDocument)parent;
        }
        else
        {
            doc = parent.OwnerDocument;
        }
        XmlElement node = doc.CreateElement(tag);

        parent.AppendChild(node);

        foreach (var v in attributes)
        {
            //创建一个属性
            XmlAttribute attribute = doc.CreateAttribute(v.Key);
            attribute.Value = v.Value;
            //xml节点附件属性
            node.Attributes.Append(attribute);
        }

        return node;
    }
#endif

    public virtual void ParseXml(SecurityElement node)
    {
        if (node == null)
        {
            return;
        }
        if (IsTypeOrSubClass(node.Tag, typeof(STComponent)))
        {
            transform.position = node.Attribute("position").ToVector3Ex();
            transform.rotation = Quaternion.Euler(node.Attribute("rotation").ToVector3Ex());
            transform.localScale = node.Attribute("position").ToVector3Ex();
        }
    }

  

    public static bool IsTypeOrSubClass(string tag,Type subClass)
    {
        if(string.IsNullOrEmpty(tag))
        {
            return false;
        }
        Type type = Type.GetType(tag);
        if(type!=null)
        {
            return type== subClass|| type.IsSubclassOf(subClass);
        }
        return false;
    }

   
}

