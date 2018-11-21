using System;
#if UNITY_EDITOR
using System.Xml;
#endif
using System.Collections.Generic;
using UnityEngine;
using System.Security;


[System.Serializable]
public class STSceneEntity: STComponent
{
    [HideInInspector] public string path = "";
   
    [HideInInspector] public Vector3 localPosition;
    [HideInInspector] public Vector3 localRotation;
    [HideInInspector] public Vector3 localScale= Vector3.one;
    [HideInInspector] public GameObject mGo;


#if UNITY_EDITOR
    public override XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes=null)
    {
        if (attributes == null)
        {
            attributes = new Dictionary<string, string>();
        }
       
        attributes.Add("localPosition", localPosition.ToString());
        attributes.Add("localRotation", localRotation.ToString());
        attributes.Add("localScale", localScale.ToString());
        attributes.Add("path", path);


        return base.ToXml(parent, attributes);
    }
#endif
    public override void UpdateAttribute()
    {
      
    }

    public override void SetAttribute()
    {
       
        if (mGo)
        {
            DestroyImmediate(mGo);
        }
        if (string.IsNullOrEmpty(path) == false)
        {
            if (Application.isPlaying == false)
            {
#if UNITY_EDITOR
                string assetName = System.IO.Path.GetFileNameWithoutExtension(path);
                gameObject.name = GetType().Name + "+" + assetName;

                var  obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                CreateEntity(obj);
#endif
            }
            else
            {
                /*
                AssetCache.LoadAssetAsync<GameObject>(path, path, (asset) =>
                {
                    if (asset == null || asset.IsLoadError())
                    {

                    }
                    else
                    {
                        asset.AddRef();
                        CreateEntity(asset.GetAsset());               
                    }
                });
                */
            }

            
        }
    }

    private void CreateEntity(UnityEngine.Object obj)
    {
        if (obj)
        {
            mGo = Instantiate(obj) as GameObject;
            mGo.transform.SetParent(transform);
            mGo.transform.localPosition = localPosition;
            mGo.transform.localRotation = Quaternion.Euler(localRotation);
            mGo.transform.localScale = localScale;
        }
    }

    public override void ParseXml(SecurityElement node)
    {
        if(node == null)
        {
            return;
        }

        base.ParseXml(node);

        if (IsTypeOrSubClass(node.Tag, typeof(STSceneEntity)))
        {
            localPosition = node.Attribute("localPosition").ToVector3Ex();
            localRotation = node.Attribute("localRotation").ToVector3Ex();
            localScale = node.Attribute("localScale").ToVector3Ex();
           
            path = node.Attribute("path");
        }

        SetAttribute();
    }
}

