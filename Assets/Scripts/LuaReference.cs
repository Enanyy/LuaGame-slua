using System;
using System.Collections.Generic;
using UnityEngine;

public class LuaReference
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


    public static int Add(GameObject go)
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
    public static void Remove(int id)
    {
        if (mObjectDic.ContainsKey(id))
        {
            mObjectDic.Remove(id);
        }

        mRemoveObjectList.Clear();
        var it = mObjectDic.GetEnumerator();
        while (it.MoveNext())
        {
            if (it.Current.Value == null)
            {
                mRemoveObjectList.Add(it.Current.Key);
            }
        }
        it.Dispose();
        for (int i = 0; i < mRemoveObjectList.Count; ++i)
        {
            if (mObjectDic.ContainsKey(mRemoveObjectList[i]))
            {
                mObjectDic.Remove(mRemoveObjectList[i]);
            }
        }
        mRemoveObjectList.Clear();

    }

    public static GameObject Get(int id)
    {
        if (mObjectDic.ContainsKey(id))
        {
            return mObjectDic[id];
        }
        return null;
    }
    public static Type GetComponentType(int type)
    {
        if (mComponentsDic.ContainsKey(type))
        {
            return mComponentsDic[type];
        }
        return typeof(Transform);
    }

    public static void Clear()
    {
        mObjectDic.Clear();
    }
}

