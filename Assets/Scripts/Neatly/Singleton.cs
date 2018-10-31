
using System;

public class Singleton<T> where T : class, new()
{
    private static T m_Instance;

   

    public static void CreateInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = Activator.CreateInstance<T>();
            (m_Instance as Singleton<T>).Init();
        }
    }

    public static void Destroy()
    {
        if (m_Instance != null)
        {
            (m_Instance as Singleton<T>).Dispose();
            m_Instance = null;
        }
    }

    public static T GetSingleton()
    {
        if (m_Instance == null)
        {
            CreateInstance();
        }
        return m_Instance;
    }

    public static bool HasInstance()
    {
        return m_Instance != null;
    }

    protected Singleton() { }

    public virtual void Init() { }

    public virtual void Dispose() { }
}
