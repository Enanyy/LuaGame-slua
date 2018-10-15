using UnityEngine;
using System.Collections;
using System;

public static class LuaHelper
{

    #region Position
    public static void SetPosition(GameObject go, float x, float y, float z)
    {
        if(go)
        {
            Vector3 position = go.transform.position;
            position.x = x;
            position.y = y;
            position.z = z;
            go.transform.position = position;
        }
    }

    public static void SetLocalPosition(GameObject go, float x, float y, float z)
    {
        if (go)
        {
            Vector3 position = go.transform.localPosition;
            position.x = x;
            position.y = y;
            position.z = z;
            go.transform.localPosition = position;
        }
    }

    public static void GetPosition(GameObject go, out float x, out float y, out float z)
    {
        x = y = z = 0;

        if (go)
        {
            x = go.transform.position.x;
            y = go.transform.position.y;
            z = go.transform.position.z;
        }
    }

    public static void GetLocalPosition(GameObject go, out float x, out float y, out float z)
    {
        x = y = z = 0;

        if (go)
        {
            x = go.transform.localPosition.x;
            y = go.transform.localPosition.y;
            z = go.transform.localPosition.z;
        }
    }

    public static void SetForward(GameObject go, float x, float y, float z)
    {
        if (go)
        {
            Vector3 forward = go.transform.forward;
            forward.x = x;
            forward.y = y;
            forward.z = z;
            go.transform.forward = forward; 
        }
    }

    public static void GetForward(GameObject go, out float x, out float y, out float z)
    {
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
    public static void SetScale(GameObject go ,float x, float y, float z)
    {
        if (go)
        {
            Vector3 scale = go.transform.localScale;
            scale.x = x;
            scale.y = y;
            scale.z = z;
            go.transform.localScale = scale;
        }
    }

    public static void GetScale(GameObject go, out float x, out float y, out float z)
    {

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
    public static void SetLocalEuler(GameObject go, float x, float y, float z)
    {
        if(go)
        {
            go.transform.localRotation = Quaternion.Euler(x, y, z);
        }
    }
    public static void GetLocalEuler(GameObject go, out float x, out float y, out float z)
    {
        x = y = z = 0;
        if (go)
        {
            x = go.transform.localEulerAngles.x;
            y = go.transform.localEulerAngles.y;
            z = go.transform.localEulerAngles.z;
        }
    }

    public static void SetLocalRotation(GameObject go, float x, float y, float z, float w)
    {
        if (go)
        {
            Quaternion q =  go.transform.localRotation;
            q.Set(x, y, z, w);
            go.transform.localRotation = q;
        }
    }

    public static void SetEuler(GameObject go, float x, float y, float z)
    {
        if (go)
        {
            go.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
    public static void GetEuler(GameObject go, float x, float y, float z)
    {
        x = y = z = 0;
        if (go)
        {
            x = go.transform.eulerAngles.x;
            y = go.transform.eulerAngles.y;
            z = go.transform.eulerAngles.z;
        }
    }

    public static void SetRotation(GameObject go, float x, float y, float z, float w)
    {
        if (go)
        {
            Quaternion q = go.transform.rotation;
            q.Set(x, y, z, w);
            go.transform.rotation = q;
        }
    }

    public static void GetRotation(GameObject go, out float x, out float y, out float z, out float w)
    {
        x = y = z = w =0;

        if (go)
        {
            x = go.transform.rotation.x;
            y = go.transform.rotation.y;
            z = go.transform.rotation.z;
            w = go.transform.rotation.w;
        }
    }

    public static void GetLocalRotation(GameObject go, out float x, out float y, out float z, out float w)
    {
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
    public static Component GetComponent(GameObject go, Type type)
    {
        if (go)
        {
            return go.GetComponent(type);
        }
        return null;
    }

    public static Component AddComponent(GameObject go, Type type)
    {
        if (go)
        {
            return go.AddComponent(type);
        }
        return null;
    }

    public static Component GetComponent(GameObject go, Type type, string path)
    {
        if (go)
        {
            Transform t = go.transform.Find(path);
            if (t)
            {
                return t.GetComponent(type);
            }
        }
        return null;
    }

    public static Component GetComponentInChildren(GameObject go, Type type)
    {
        if (go)
        {
            return go.GetComponentInChildren(type);
        }
        return null;
    }

    #endregion

    public static int GetChildCount(GameObject go)
    {
        if(go)
        {
            return go.transform.childCount;
        }
        return 0;
    }

    public static GameObject GetChild(GameObject go, int index)
    {
        if (go)
        {
            if (index < go.transform.childCount )
            {
                return go.transform.GetChild(index).gameObject;
            }
        }
        return null;
    }

    public static void SetParent(GameObject go, Transform parent)
    {
        if (go)
        {
            go.transform.SetParent(parent);
        }
    }

    public static void SetActive(GameObject go, int active)
    {
        if (go)
        {
            go.SetActive(active == 1);
        }
    }

    #region NGUI

    #region AddClick
    public static void AddClick(UIButton button, SLua.LuaFunction function)
    {
        if(button)
        {
            button.onClick.Add(new EventDelegate(delegate () {
                if(function!=null)
                {
                    function.call();
                }
            }));
        }
    }
    public static void AddClick(GameObject go, SLua.LuaFunction function)
    {
        if (go == null)
        {
            return;
        }
       
        AddClick(go.transform, function);
    }
    public static void AddClick(Transform transform, SLua.LuaFunction function)
    {
        if(transform == null)
        {
            return;
        }
        var button = transform.GetComponent<UIButton>();

        AddClick(button, function);
    
    }

    public static void AddClick(Transform transform, string path, SLua.LuaFunction function)
    {
        Transform child = GetComponent(transform.gameObject, typeof(Transform), path) as Transform;
        if(child)
        {
            AddClick(child, function);
        }

    }
    #endregion
    #region SetText
    public static void SetText(UILabel label, string text)
    {
        if(label)
        {
            label.text = text;
        }
    }
    public static void SetText(Transform transform,string text)
    {
        if(transform)
        {
            UILabel label = transform.GetComponent<UILabel>();
            SetText(label, text);
        }
    }
    public static void SetText(GameObject go, string text)
    {
        if (go)
        {
            UILabel label = go.GetComponent<UILabel>();
            SetText(label, text);
        }
    }
    public static void SetText(Transform transform, string path, string text)
    {
        Transform child = GetComponent(transform.gameObject, typeof(Transform), path) as Transform;
        if (child)
        {
            SetText(child, text);
        }
    }
    #endregion

    #region SetSprite
    public static void SetSprite(UISprite sprite, string spriteName, bool native)
    {
        if(sprite)
        {
            sprite.spriteName = spriteName;

            if(native)
            {
                sprite.MakePixelPerfect();
            }
        }
    }

    public static void SetSprite(Transform transform, string spriteName, bool native)
    {
        if(transform)
        {
            UISprite sprite = transform.GetComponent<UISprite>();
            SetSprite(sprite, spriteName, native);
        }
    }

    public static void SetSprite(Transform transform, string path, string spriteName, bool native)
    {
        Transform child = GetComponent(transform.gameObject, typeof(Transform), path) as Transform;
        if (child)
        {
            SetSprite(child, spriteName, native);
        }
    }
    public static void SetSprite(GameObject go, string spriteName, bool native)
    {
        if (go)
        {
            SetSprite(go.transform, spriteName, native);
        }
    }

    #endregion


    #endregion

    public static UnityEngine.Object LoadAsset(string path)
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
#else
        return null;
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
   
}
