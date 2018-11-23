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
   
   
    [HideInInspector] public GameObject mGo;


#if UNITY_EDITOR
    public override XmlElement ToXml(XmlNode parent, Dictionary<string, string> attributes=null)
    {
        if (attributes == null)
        {
            attributes = new Dictionary<string, string>();
        }
       
     
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

                
                if (obj)
                {
                    //还原出其预设
                    mGo = UnityEditor.PrefabUtility.InstantiatePrefab(obj) as GameObject;
  
                    mGo.transform.SetParent(transform);
                    mGo.transform.localPosition = Vector3.zero;
                    mGo.transform.localRotation = Quaternion.identity;
                    mGo.transform.localScale = Vector3.one;
                }
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
            mGo.transform.localPosition = Vector3.zero;
            mGo.transform.localRotation = Quaternion.identity;
            mGo.transform.localScale = Vector3.one;
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
            path = node.Attribute("path");
        }

        SetAttribute();
    }
}

