using UnityEngine;
using System.Collections.Generic;
using System;

public static class LuaHelper
{
    private static Dictionary<int, GameObject> mObjectDic = new Dictionary<int, GameObject>();
    private static List<int> mRemoveObjectList = new List<int>();
    private static int mObjectID = 0;
    public const int INVALID_GAMEOBJECT_ID = -1;

    private static Dictionary<int, Type> mComponentsDic = new Dictionary<int, Type> {
        {0,typeof(GameObject) },
        {1,typeof(Transform) },
        {2,typeof(Camera) },
        {3,typeof(Animation) },
        {4,typeof(BoxCollider) },

        //NGUI
        {100,typeof(UIRoot) },
        {101,typeof(UICamera) },
        {102,typeof(UIPanel) },
        {103,typeof(UIWidget) },
        {104,typeof(UIButton) },
        {105,typeof(UISprite) },
        {106,typeof(UITable) },
        {107,typeof(UIGrid) },
        {108,typeof(UIScrollView) },

        //Custom
        {200,typeof(BlurEffect) },

    };

    
    private static int Add(GameObject go)
    {
        //找到一个没用的ID
        while (true)
        {
            mObjectID++;
            if (mObjectID == int.MaxValue)
            {
                mObjectID = 0;
            }
            else
            {
                if (mObjectDic.ContainsKey(mObjectID) == false)
                {
                    break;
                }
            }
        }
        mObjectDic.Add(mObjectID, go);
        
        return mObjectID;
    }
    private static void Remove(int id)
    {
        if(mObjectDic.ContainsKey(id))
        {
            mObjectDic.Remove(id);
        }

        mRemoveObjectList.Clear();
        var it = mObjectDic.GetEnumerator();
        while(it.MoveNext())
        {
            if(it.Current.Value ==null)
            {
                mRemoveObjectList.Add(it.Current.Key);
            }
        }
        it.Dispose();
        for(int i =0; i <mRemoveObjectList.Count;++i)
        {
            if (mObjectDic.ContainsKey(mRemoveObjectList[i]))
            {
                mObjectDic.Remove(mRemoveObjectList[i]);
            }
        }
        mRemoveObjectList.Clear();

    }

    private static GameObject Get(int id)
    {
        if(mObjectDic.ContainsKey(id))
        {
            return mObjectDic[id];
        }
        return null;
    }
    private static Type GetComponentType(int type)
    {
        if(mComponentsDic.ContainsKey(type))
        {
            return mComponentsDic[type];
        }
        return typeof(Transform);
    }

    #region GameObject

    public static int GameObject(string name)
    {
        var go = new GameObject(name);
        return Add(go);
    }

    public static int Instantiate(int id)
    {
        var go = Get(id);
        if (go)
        {
            return Add(UnityEngine.Object.Instantiate(go));
        }
        return INVALID_GAMEOBJECT_ID;
    }

    public static void DontDestroyOnLoad(int id)
    {
        var go = Get(id);
        if (go)
        {
            UnityEngine.Object.DontDestroyOnLoad(go);
        }
    }
    public static void Destroy(int id)
    {
        var go = Get(id);
        if (go)
        {
            UnityEngine.Object.Destroy(go);
            Remove(id);
        }
    }
    public static void DestroyImmediate(int id)
    {
        var go = Get(id);
        if (go)
        {
            UnityEngine.Object.DestroyImmediate(go);
            Remove(id);
        }

    }
    public static void DestroyComponent(int id,int type)
    {
        var go = Get(id);
        if (go)
        {
            var componet = go.GetComponent(GetComponentType(type));

            if (componet)
            {
                UnityEngine.Object.DestroyImmediate(componet);
            }
        }
    }


    #region Position
    public static void SetPosition(int id, float x, float y, float z)
    {
        var go = Get(id);
        if(go)
        {
            Vector3 position = go.transform.position;
            position.x = x;
            position.y = y;
            position.z = z;
            go.transform.position = position;
        }
    }

    public static void SetLocalPosition(int id, float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            Vector3 position = go.transform.localPosition;
            position.x = x;
            position.y = y;
            position.z = z;
            go.transform.localPosition = position;
        }
    }

    public static void GetPosition(int id, out float x, out float y, out float z)
    {
        var go = Get(id);
       
        x = y = z = 0;

        if (go)
        {
            x = go.transform.position.x;
            y = go.transform.position.y;
            z = go.transform.position.z;
        }
    }

    public static void GetLocalPosition(int id, out float x, out float y, out float z)
    {
        var go = Get(id);

        x = y = z = 0;

        if (go)
        {
            x = go.transform.localPosition.x;
            y = go.transform.localPosition.y;
            z = go.transform.localPosition.z;
        }
    }

    public static void SetForward(int id, float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            Vector3 forward = go.transform.forward;
            forward.x = x;
            forward.y = y;
            forward.z = z;
            go.transform.forward = forward; 
        }
    }

    public static void GetForward(int id, out float x, out float y, out float z)
    {
        var go = Get(id);
        x = y = z = 0;
        if (go)
        {
            x = go.transform.forward.x;
            y = go.transform.forward.y;
            z = go.transform.forward.z;
        }
    }

    #endregion
    #region Scale
    public static void SetScale(int id ,float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            Vector3 scale = go.transform.localScale;
            scale.x = x;
            scale.y = y;
            scale.z = z;
            go.transform.localScale = scale;
        }
    }

    public static void GetScale(int id, out float x, out float y, out float z)
    {
        var go = Get(id);
        x = y = z = 0;

        if (go)
        {
            x = go.transform.localScale.x;
            y = go.transform.localScale.y;
            z = go.transform.localScale.z;
        }
    }
    #endregion
    #region Rotation
    public static void SetLocalEuler(int id, float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            go.transform.localRotation = Quaternion.Euler(x, y, z);
        }
    }
    public static void GetLocalEuler(int id, out float x, out float y, out float z)
    {
        var go = Get(id);
        x = y = z = 0;
        if (go)
        {
            x = go.transform.localEulerAngles.x;
            y = go.transform.localEulerAngles.y;
            z = go.transform.localEulerAngles.z;
        }
    }

    public static void SetLocalRotation(int id, float x, float y, float z, float w)
    {
        var go = Get(id);
        if (go)
        {
            Quaternion q =  go.transform.localRotation;
            q.Set(x, y, z, w);
            go.transform.localRotation = q;
        }
    }

    public static void SetEuler(int id, float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            go.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
    public static void GetEuler(int id, float x, float y, float z)
    {
        var go = Get(id);
        x = y = z = 0;
        if (go)
        {
            x = go.transform.eulerAngles.x;
            y = go.transform.eulerAngles.y;
            z = go.transform.eulerAngles.z;
        }
    }

    public static void SetRotation(int id, float x, float y, float z, float w)
    {
        var go = Get(id);
        if (go)
        {
            Quaternion q = go.transform.rotation;
            q.Set(x, y, z, w);
            go.transform.rotation = q;
        }
    }

    public static void GetRotation(int id, out float x, out float y, out float z, out float w)
    {
        var go = Get(id);
        x = y = z = w =0;

        if (go)
        {
            x = go.transform.rotation.x;
            y = go.transform.rotation.y;
            z = go.transform.rotation.z;
            w = go.transform.rotation.w;
        }
    }

    public static void GetLocalRotation(int id, out float x, out float y, out float z, out float w)
    {
        var go = Get(id);

        x = y = z = w = 0;

        if (go)
        {
            x = go.transform.localRotation.x;
            y = go.transform.localRotation.y;
            z = go.transform.localRotation.z;
            w = go.transform.localRotation.w;
        }
    }

    #endregion
    #region Component
   

    public static int AddComponent(int id, int type)
    {
        var go = Get(id);
        if (go)
        {
            go.AddComponent(GetComponentType(type));
            return id;
        }
        return INVALID_GAMEOBJECT_ID;
    }
   
    public static int FindChildWithComponent(int id, int type)
    {
        var go = Get(id);
        if (go)
        {
            var component = go.GetComponentInChildren(GetComponentType(type));
            if(component)
            {
                return Add(component.gameObject);
            }
        }
        return INVALID_GAMEOBJECT_ID;
    }

    public static int IsEnable(int id,int type)
    {
        var go = Get(id);
        if (go)
        {
            var behaviour = go.GetComponent(GetComponentType(type)) as Behaviour;
            if (behaviour)
            {
              return  behaviour.enabled ? 1:0;
            }
        }
        return 0;
    }

    public static void SetEnable(int id, int type, int enable)
    {
        var go = Get(id);
        if (go)
        {
            var behaviour = go.GetComponent(GetComponentType(type)) as Behaviour;
            if(behaviour)
            {
                behaviour.enabled = enable == 1;
            }
        }
    }

    private static T GetComponent<T>(int id, string path) where T: Component
    {
        var go = Get(id);
        if (go)
        {
            Transform t = go.transform.Find(path);
            if (t)
            {
                return t.GetComponent<T>();
            }
        }
        return null;
    }

    private static T FindChildWithComponent<T>(int id) where T : Component
    {
        var go = Get(id);
        if (go)
        {
            return go.GetComponentInChildren<T>();
        }
        return null;
    }

    

    public static int GetChildCount(int id)
    {
        var go = Get(id);
        if (go)
        {
            return go.transform.childCount;
        }
        return 0;
    }

    public static int GetChild(int id, int index)
    {
        var go = Get(id);
        if (go)
        {
            if (index < go.transform.childCount )
            {
                return Add(go.transform.GetChild(index).gameObject);
            }
        }
        return INVALID_GAMEOBJECT_ID;
    }

    public static int FindChild(int id, string path)
    {
        var go = Get(id);
        if (go)
        {
            var child = go.transform.Find(path);
            if(child)
            {
                return Add(child.gameObject);
            }
        }
        return INVALID_GAMEOBJECT_ID;
    }

    public static void SetParent(int id, int parent)
    {
        var go = Get(id);
        if (go)
        {
            var p = Get(parent);

            go.transform.SetParent(p!=null?p.transform:null);
        }
    }
    public static int GetParent(int id)
    {
        var go = Get(id);
        if (go && go.transform.parent)
        {
            return Add(go.transform.parent.gameObject);
        }
        return INVALID_GAMEOBJECT_ID;
    }

    public static void SetActive(int id, int active)
    {
        var go = Get(id);

        if (go)
        {
            go.SetActive(active == 1);
        }
    }
    public static int IsActive(int id)
    {
        var go = Get(id);

        if (go)
        {
            return go.activeInHierarchy ? 1 : 0;
        }
        return 0;
    }

    public static void SetLayer(int id, int layer)
    {
        var go = Get(id);

        if (go)
        {
            NGUITools.SetLayer(go, layer);
        }
    }
    public static void SetAsFirstSibling(int id)
    {
        var go = Get(id);

        if (go)
        {
            go.transform.SetAsFirstSibling();
        }
    }
    public static void SetAsLastSibling(int id)
    {
        var go = Get(id);

        if (go)
        {
            go.transform.SetAsLastSibling();
        }
    }
    public static void SetSiblingIndex(int id, int index)
    {
        var go = Get(id);

        if (go)
        {
            go.transform.SetSiblingIndex(index);
        }
    }
    public static int GetSiblingIndex(int id)
    {
        var go = Get(id);

        if (go)
        {
            return go.transform.GetSiblingIndex();
        }
        return -1;
    }
    public static void InverseTransformDirection(int id, float x, float y, float z, out float ox, out float oy, out float oz)
    {
        var go = Get(id);

        ox = oy = oz = 0;

        if (go)
        {
            Vector3 pos = go.transform.InverseTransformDirection(x, y, z);
            ox = pos.x;
            oy = pos.y;
            oz = pos.z;
        }
    }

    public static void InverseTransformPoint(int id, float x, float y, float z, out float ox, out float oy, out float oz)
    {
        var go = Get(id);

        ox = oy = oz = 0;

        if (go)
        {
            Vector3 pos = go.transform.InverseTransformPoint(x, y, z);
            ox = pos.x;
            oy = pos.y;
            oz = pos.z;
        }
    }

    public static void InverseTransformVector(int id, float x, float y, float z, out float ox, out float oy, out float oz)
    {
        var go = Get(id);

        ox = oy = oz = 0;

        if (go)
        {
            Vector3 pos = go.transform.InverseTransformVector(x, y, z);
            ox = pos.x;
            oy = pos.y;
            oz = pos.z;
        }
    }

    public static void TransformDirection(int id, float x, float y, float z, out float ox, out float oy, out float oz)
    {
        var go = Get(id);

        ox = oy = oz = 0;

        if (go)
        {
            Vector3 pos = go.transform.TransformDirection(x, y, z);
            ox = pos.x;
            oy = pos.y;
            oz = pos.z;
        }
    }
    public static void TransformPoint(int id, float x, float y, float z, out float ox, out float oy, out float oz)
    {
        var go = Get(id);

        ox = oy = oz = 0;

        if (go)
        {
            Vector3 pos = go.transform.TransformPoint(x, y, z);
            ox = pos.x;
            oy = pos.y;
            oz = pos.z;
        }
    }
    public static void TransformVector(int id, float x, float y, float z, out float ox, out float oy, out float oz)
    {
        var go = Get(id);

        ox = oy = oz = 0;

        if (go)
        {
            Vector3 pos = go.transform.TransformVector(x, y, z);
            ox = pos.x;
            oy = pos.y;
            oz = pos.z;
        }
    }

    public static void LookAt(int id, float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            go.transform.LookAt(new Vector3(x, y, z));
        }
    }

    public static void Rotate(int id, float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            go.transform.Rotate(x, y, z);
        }
    }
    public static void Translate(int id, float x, float y, float z)
    {
        var go = Get(id);
        if (go)
        {
            go.transform.Translate(x, y, z);
        }
    }
    #endregion
    #endregion

    #region Camera

    public static void SetCameraCullingMask(int id, int mask)
    {
        var go = Get(id);

        if (go)
        {
            Camera camera = go.GetComponent<Camera>();
            if(camera)
            {
                camera.cullingMask = mask;
            }
        }
    }

    public static void SetCameraDepth(int id, int depth)
    {
        var go = Get(id);

        if (go)
        {
            Camera camera = go.GetComponent<Camera>();
            if (camera)
            {
                camera.depth = depth;
            }
        }
    }

    #endregion

    #region NGUI

    public static int CreateUI(int width, int height)
    {
        var p = NGUITools.CreateUI(false);

        var uiCamera = p.GetComponentInChildren<UICamera>();
        var camera = uiCamera.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.Depth;
      
        UnityEngine.Object.DestroyImmediate(camera.GetComponent<AudioListener>());

        var uiRoot = p.GetComponent<UIRoot>();
        uiRoot.scalingStyle = UIRoot.Scaling.Constrained;
        uiRoot.manualWidth = width;
        uiRoot.manualHeight = height;

        float screenAspectRatio = (Screen.width * 1.0f) / Screen.height;
        float designAspectRatio = (width * 1.0f) / height;
        if ((screenAspectRatio * 100) < (designAspectRatio * 100))
        {
            uiRoot.fitWidth = true;
            uiRoot.fitHeight = false;
        }
        else if ((screenAspectRatio * 100) > (designAspectRatio * 100))
        {
            uiRoot.fitWidth = false;
            uiRoot.fitHeight = true;
        }

        return Add(p.gameObject);
    }
    public static void SetUITouchable(int id, int touchable)
    {
        var go = Get(id);

        if (go)
        {
            UICamera uiCamera = go.GetComponent<UICamera>();
            if(uiCamera)
            {
                uiCamera.useTouch = touchable== 1;
                uiCamera.useMouse = touchable == 1;
            }
        }
    }
    #region AddClick
   
    public static void AddClick(int id, SLua.LuaFunction function)
    {
        var go = Get(id);

        if (go == null)
        {
            return;
        }
       
        AddClick(go.transform, function);
    }
    private static void AddClick(Transform transform, SLua.LuaFunction function)
    {
        if(transform == null)
        {
            return;
        }
        var button = transform.GetComponent<UIButton>();

        if (button)
        {
            button.onClick.Add(new EventDelegate(delegate ()
            {
                if (function != null)
                {
                    function.call();
                }
            }));
        }

    }

    public static void AddClick(int id, string path, SLua.LuaFunction function)
    {
        Transform child = GetComponent<Transform>(id, path);
        if(child)
        {
            AddClick(child, function);
        }

    }
    #endregion
    #region SetText
   
    private static void SetText(Transform transform,string text)
    {
        if(transform)
        {
            UILabel label = transform.GetComponent<UILabel>();
            if (label)
            {
                label.text = text;
            }
        }
    }
    public static void SetText(int id, string text)
    {
        var go = Get(id);
        if (go)
        {
            SetText(go.transform, text);
        }
    }
    public static void SetText(int id, string path, string text)
    {
        Transform child = GetComponent<Transform>(id, path);
        if (child)
        {
            SetText(child, text);
        }
    }
    #endregion

    #region SetSprite
 
    private static void SetSprite(Transform transform, string spriteName, bool native)
    {
        if(transform)
        {
            UISprite sprite = transform.GetComponent<UISprite>();

            if (sprite)
            {
                sprite.spriteName = spriteName;

                if (native)
                {
                    sprite.MakePixelPerfect();
                }
            }
        }
    }

    public static void SetSprite(int id, string path, string spriteName, bool native)
    {
        Transform child = GetComponent<Transform>(id, path);
        if (child)
        {
            SetSprite(child, spriteName, native);
        }
    }
    public static void SetSprite(int id, string spriteName, bool native)
    {
        var go = Get(id);
        if (go)
        {
            SetSprite(go.transform, spriteName, native);
        }
    }

    #endregion

    #region UIPanel

    public static void SetPanelAlpha(int id, float alpha)
    {
        var go = Get(id);
        if (go)
        {
            UIPanel panel = go.GetComponent<UIPanel>();
            if(panel)
            {
                panel.alpha = alpha;
            }
        }
    }
    public static float GetPanelAlpha(int id)
    {
        var go = Get(id);
        if (go)
        {
            UIPanel panel = go.GetComponent<UIPanel>();
            if (panel)
            {
              return  panel.alpha;
            }
        }
        return 0;
    }
    public static int GetPanelDepth(int id)
    {
        var go = Get(id);
        if (go)
        {
            UIPanel panel = go.GetComponent<UIPanel>();
            if (panel)
            {
                return panel.depth;
            }
        }
        return 0;
    }
    public static void SetPanelDepth(int id,int depth)
    {
        var go = Get(id);
        if (go)
        {
            UIPanel panel = go.GetComponent<UIPanel>();
            if (panel)
            {
                 panel.depth = depth;
            }
        }
 
    }
    #endregion
    #region UIWidget
    public static int GetWidgetDepth(int id)
    {
        var go = Get(id);
        if (go)
        {
            UIWidget widget = go.GetComponent<UIWidget>();
            if (widget)
            {
                return widget.depth;
            }
        }
        return 0;
    }
    public static void SetWidgetDepth(int id, int depth)
    {
        var go = Get(id);
        if (go)
        {
            UIWidget widget = go.GetComponent<UIWidget>();
            if (widget)
            {
                widget.depth = depth;
            }
        }

    }

    public static void ResizeCollider(int id)
    {
        var go = Get(id);
        if (go)
        {
            UIWidget widget = go.GetComponent<UIWidget>();
            if (widget)
            {
                widget.ResizeCollider();
            }
        }
    }

    public static void SetAnchor(int id, int anchor, int left, int bottom, int right, int top)
    {
        var go = Get(id);
        var to = Get(anchor);
        if (go && to)
        {
            UIWidget widget = go.GetComponent<UIWidget>();
            if (widget)
            {
                widget.SetAnchor(to.gameObject, left, bottom, right, top);
            }
        }
    }

    public static void SetWidgetSize(int id, int width, int height)
    {
        var go = Get(id);
        if (go)
        {
            UIWidget widget = go.GetComponent<UIWidget>();
            if (widget)
            {
                widget.width = width;
                widget.height = height;
            }
        }
    }
            


    #endregion
    #endregion

    public static int  LoadAsset(string path)
    {
#if UNITY_EDITOR
        var asset= UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        if(asset)
        {
            var go = UnityEngine.Object.Instantiate(asset) as GameObject;
            return Add(go);
        }
        return INVALID_GAMEOBJECT_ID;
#else
        return INVALID_GAMEOBJECT_ID;
#endif

    }

    public static void ProtobufString(SLua.ByteArray data)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(data.GetData(), 0, data.Position);
       
        PBMessage.Person person = ProtoTransfer.DeserializeProtoBuf<PBMessage.Person>(ms);
        Debug.Log("age=" + person.age);
        Debug.Log("email=" + person.email);
        Debug.Log("name=" + person.name);
        Debug.Log("id=" + person.id);
        var table = SLua.LuaSvr.mainState.getFunction("TestParseProtobuf");
        if(table!=null)
        {
            table.call( data);
        }
    }
   
    public static SLua.ByteArray LoadLuaAsset(string name)
    {
        string file = LuaFile.FindFile(name);
        if(string.IsNullOrEmpty(file)==false)
        {
            byte[] bytes = LuaFile.ReadBytes(file);
            SLua.ByteArray array = new SLua.ByteArray();
            array.Write(bytes);

            return array;
        }
        return null;
    }
}
