using System;
using System.Collections.Generic;
using System.Security;
#if UNITY_EDITOR
using System.Xml;
#endif
using UnityEngine;
public class STSceneGroup : STComponent
{  
    [HideInInspector]public string groupName { get {return gameObject.name; } set { gameObject.name = value; } }
    [HideInInspector]public List<STComponent> components = new List<STComponent>();

    public override void CheckFinish(out int count, out int finishCount)
    {
        count = 0;
        finishCount = 0;
        for (int i = 0; i <components.Count; ++i)
        {
            int f = 0;
            int c = 0;
            components[i].CheckFinish(out c, out f);
            finishCount += f;
            count += c;
        }
    }

    public override void ParseXml(SecurityElement node)
    {
        if (node == null)
        {
            return;
        }
        base.ParseXml(node);
        if (IsTypeOrSubClass(node.Tag, typeof(STSceneGroup)))
        {
            groupName = node.Attribute("name");

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
    }

    public override void SetAttribute()
    {
        for(int i = 0; i < components.Count;++i)
        {
            components[i].SetAttribute();
        }
    }

#if UNITY_EDITOR
    public override XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes=null)
    {
        if (attributes == null)
        {
            attributes = new Dictionary<string, string>();
        }
        attributes.Add("name", groupName);

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
    

   
    public void AddSTComponent(STComponent component)
    {
        if (components.Contains(component) == false)
        {
            components.Add(component);
        }
    }
}

