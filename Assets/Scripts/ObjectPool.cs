using System;
using System.Collections.Generic;
public interface IPool
{
    bool isPool { get; set; }

    void OnCreate();
    void OnDestroy();
    void OnRecycle();
}
public static class ObjectPool 
{
    static Dictionary<Type, Queue<IPool>> mPoolDic = new Dictionary<Type, Queue<IPool>>(); 
    public static T GetInstance<T>(Type type = null) where T: IPool
    {
        if(type== null)
        {
            type = typeof(T);
        }
        if(mPoolDic.ContainsKey(type)==false)
        {
            mPoolDic.Add(type, new Queue<IPool>());
        }
        IPool o = null;
        if(mPoolDic[type].Count >0)
        {
            o = mPoolDic[type].Dequeue();
        }
        else
        {
            o = (IPool)Activator.CreateInstance(type);       
        }
        o.OnCreate();
        o.isPool = false;
        
        return (T)o;
    }

    public static void RecycleInstance<T>(T o) where T:IPool
    {
        RecycleInstance(o, typeof(T));
    }

    public static void RecycleInstance(IPool o, Type type )
    {
        if(o==null)
        {
            return;
        }
        if (mPoolDic.ContainsKey(type) == false)
        {
            mPoolDic.Add(type, new Queue<IPool>());
        }
        if (mPoolDic[type].Contains(o) == false)
        {
            o.isPool = true;
            o.OnRecycle();
            mPoolDic[type].Enqueue(o);
        }
    }

    public static void Clear(Type type, bool includeSubClass = false)
    {
        if (includeSubClass)
        {
            var it = mPoolDic.GetEnumerator();
            List<Type> list = new List<Type>();
            while (it.MoveNext())
            {
                if (it.Current.Key.IsSubclassOf(type))
                {
                    list.Add(type);
                    var queue = it.Current.Value;
                    while(queue.Count >0)
                    {
                        IPool o = queue.Dequeue();
                        o.OnDestroy();
                    }
                }
            }
            for(int i = 0; i < list.Count;++i)
            {
                mPoolDic.Remove(list[i]);
            }
        }

        if (mPoolDic.ContainsKey(type))
        {
            var queue = mPoolDic[type];
            while (queue.Count > 0)
            {
                IPool o = queue.Dequeue();
                o.OnDestroy();
            }
            mPoolDic.Remove(type);
        }

    }

    public static void Clear<T>(bool includeSubClass = false)
    {
        Clear(typeof(T), includeSubClass);
    }

    public static void Clear()
    {
        var it = mPoolDic.GetEnumerator();
        while(it.MoveNext())
        {
            var queue = it.Current.Value;
            while (queue.Count > 0)
            {
                IPool o = queue.Dequeue();
                o.isPool = false;
                o.OnDestroy();
            }
        }
        mPoolDic.Clear();
    }
}