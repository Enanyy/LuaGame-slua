
using System;

public class Singleton<T> where T : class, new()
{
    private static T m_Instance;

    public static T instance
    {
        get {
            if(m_Instance == null)
            {
                CreateInstance();
            }
            return m_Instance;
        }
    }
  
    public static void CreateInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = Activator.CreateInstance<T>();
        }
    }

    public static void Destroy()
    {
        if (m_Instance != null)
        {
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

   

    protected Singleton() { }

}
