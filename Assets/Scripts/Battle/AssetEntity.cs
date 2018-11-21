using UnityEngine;
using UnityEditor;

public class AssetEntity :IPool
{
    public GameObject gameObject { get; private set; }

    public bool isPool { get; set; }

    public AssetEntity()
    {
        gameObject = new GameObject(GetType().Name);
    }

    public void LoadAsset(string assetBundleName,string assetName)
    {

    }
    protected virtual void OnLoadAsset(GameObject go)
    {
    }

        public void OnCreate()
    {
       
    }

    public void OnDestroy()
    {
       
    }

    public void OnRecycle()
    {
        
    }

    public virtual void Recycle()
    {

    }
}