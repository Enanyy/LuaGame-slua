using UnityEngine;

public class AssetEntity :IPool
{
    public GameObject gameObject { get; private set; }
    public GameObject asset { get; private set; }

    public bool isPool { get; set; }
    public Vector3 position { get { return gameObject.transform.position; } set { gameObject.transform.position= value; } }
    public Quaternion rotation { get { return gameObject.transform.rotation; } set { gameObject.transform.rotation = value; } }
    public Vector3 forward { get { return gameObject.transform.forward; } set { gameObject.transform.forward = value; } }
    public Vector3 right { get { return gameObject.transform.right; } set { gameObject.transform.right = value; } }
    public Vector3 up { get { return gameObject.transform.up; } set { gameObject.transform.up = value; } }
    public Vector3 eulerAngles { get { return gameObject.transform.eulerAngles; }set { gameObject.transform.eulerAngles = value; } }
    public Vector3 localPosition { get { return gameObject.transform.localPosition; } set { gameObject.transform.localPosition = value; } }
    public Vector3 localScale { get { return gameObject.transform.localScale; } set { gameObject.transform.localScale = value; } }
    public Quaternion localRotation { get { return gameObject.transform.localRotation; }set { gameObject.transform.localRotation = value; } }
    public Transform parent { get {return gameObject.transform.parent; } set { gameObject.transform.parent = value; } }

    public AssetEntity()
    {
        gameObject = new GameObject(GetType().Name);
    }

    public void LoadAsset(string assetBundleName, string assetName, System.Action<GameObject> callback = null)
    {
        var go = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetName);
        if (go)
        {
            asset = Object.Instantiate(go) as GameObject;
            asset.transform.SetParent(gameObject.transform);
            asset.transform.localPosition = Vector3.zero;
            asset.transform.localRotation = Quaternion.identity;
            asset.transform.localScale = Vector3.one;
            asset.SetActive(true);
            OnLoadAsset(asset);
            if (callback != null)
            {
                callback(asset);
            }
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