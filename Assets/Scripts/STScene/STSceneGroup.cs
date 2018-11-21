using System;
using System.Collections.Generic;
using System.Security;
#if UNITY_EDITOR
using System.Xml;
#endif
using UnityEngine;
public class STSceneGroup : STComponent
{  
    [HideInInspector]public string name;
    [HideInInspector]public List<STComponent> components = new List<STComponent>();

    public override void ParseXml(SecurityElement node)
    {
        if (node == null)
        {
            return;
        }
        base.ParseXml(node);
        if (IsTypeOrSubClass(node.Tag, typeof(STSceneGroup)))
        {
            name = node.Attribute("name");

            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; ++i)
                {
                    SecurityElement child = node.Children[i] as SecurityElement;

                    GameObject go = new GameObject(child.Tag);

                    go.transform.SetParent(transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;

                    Type component = Type.GetType(child.Tag);
                    if (component == null)
                    {
                        DestroyImmediate(go); continue;
                    }
                    STComponent entity = go.AddComponent(component) as STComponent;

                    if (entity == null)
                    {
                        DestroyImmediate(go); continue;
                    }

                    components.Add(entity);

                    entity.ParseXml(child);

                }
            }
        }

        SetAttribute();
    }
#if UNITY_EDITOR
    public override XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes=null)
    {
        if (attributes == null)
        {
            attributes = new Dictionary<string, string>();
        }
        attributes.Add("name", name);

        XmlElement node = base.ToXml(parent,attributes);
        for (int i = 0; i < components.Count; ++i)
        {
            if (components[i] != null)
            {
                components[i].ToXml(node);
            }
        }

        return node;
    }
#endif
    public override void SetAttribute()
    {
        gameObject.name = name;
    }

    public override void UpdateAttribute()
    {
        
    }
    public void AddSTComponent(STComponent component)
    {
        if (components.Contains(component) == false)
        {
            components.Add(component);
        }
    }
}

