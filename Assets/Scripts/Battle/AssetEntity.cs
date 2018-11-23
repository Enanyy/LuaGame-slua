using UnityEngine;

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
        var o= UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetName);
        if(o)
        {
            GameObject go = Object.Instantiate(o) as GameObject;
            go.transform.SetParent(gameObject.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            OnLoadAsset(go);
        }
    }
    protected virtual void OnLoadAsset(GameObject go)
    {
    }

    public void OnCreate()
    {
        gameObject.SetActive(true);   
    }

    public void OnDestroy()
    {
       if(gameObject)
        {
            Object.DestroyImmediate(gameObject);
        }
    }

    public void OnRecycle()
    {
        gameObject.SetActive(false);
    }

    public virtual void Recycle()
    {
        ObjectPool.RecycleInstance(this, GetType());
    }
}