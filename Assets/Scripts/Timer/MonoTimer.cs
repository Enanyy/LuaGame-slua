﻿
using System;
using System.Collections.Generic;
using UnityEngine;


public class MonoTimer : MonoBehaviour
{
    private static MonoTimer m_Instance;

    public static MonoTimer Init()
    {
        if (m_Instance == null)
        {
            GameObject go = new GameObject("MonoTimer");
            m_Instance = go.AddComponent<MonoTimer>();
            DontDestroyOnLoad(go);
        }
        return m_Instance;
    }

    private TimerPool timerPool = new TimerPool();
    private List<TimerImplement> timers = new List<TimerImplement>();

    public static void AddFrame(TimerBehaviour behaviour, Action<float> action, float intervalFrame = 1, bool once = false)
    {
        var timerImp = m_Instance.timerPool.Create();
        m_Instance.timers.Add(timerImp.InitFrame(behaviour, action, intervalFrame, once));
    }

    public static void AddClock(TimerBehaviour behaviour, Action<float> action, float intervalClock = 1, bool once = false)
    {
        var timerImp = m_Instance.timerPool.Create();
        m_Instance.timers.Add(timerImp.InitClock(behaviour, action, intervalClock, once));
    }

    public static TimerImplement CheckReapeat(TimerBehaviour behaviour, Action<float> action)
    {
        for (int i = 0; i < m_Instance.timers.Count; i++)
        {
            if (m_Instance.timers[i].behaviour == behaviour && m_Instance.timers[i].function == action)
            {
                return m_Instance.timers[i];
            }
        }
        return null;
    }

    public static void Remove(TimerBehaviour behaviour)
    {
        var list = m_Instance.timers;
        for (int i = 0; i < list.Count; i++)
        {
            var t = list[i];
            if (t.behaviour == behaviour)
            {
                t.SetDestroy();
            }
        }
    }

    public static void Remove(GameObject gameObject)
    {
        var list = m_Instance.timers;
        for (int i = 0; i < list.Count; i++)
        {
            var t = list[i];
            if (t.gameObject == gameObject)
            {
                t.SetDestroy();
            }
        }
    }

    public static void Remove(TimerBehaviour behaviour, Action<float> action)
    {
        var list = m_Instance.timers;
        for (int i = 0; i < list.Count; i++)
        {
            var t = list[i];
            if (t.behaviour == behaviour && t.function == action)
            {
                t.SetDestroy();
            }
        }
    }

    public static void Remove(GameObject gameObject, Action<float> action)
    {
        var list = m_Instance.timers;
        for (int i = 0; i < list.Count; i++)
        {
            var t = list[i];
            if (t.gameObject == gameObject && t.function == action)
            {
                t.SetDestroy();
            }
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            if (timers[i].IsDestroy)
            {
                timerPool.Recycle(timers[i]);
                timers.RemoveAt(i);
                continue;
            }
            timers[i].Excute(deltaTime);
        }
    }
}

internal sealed class TimerPool
{
    List<TimerImplement> pool = new List<TimerImplement>();

    public TimerImplement Create()
    {
        TimerImplement imp;
        if (pool.Count <= 0)
        {
            imp = new TimerImplement();
            return imp;
        }
        imp = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        return imp;
    }

    public void Recycle(TimerImplement imp)
    {
        imp.Dispose();
        pool.Add(imp);
    }
}

public sealed class TimerImplement
{
    public enum SaveMode
    {
        Behaviour,
        GameObject,
    }
    public enum TimerMode
    {
        Frame,
        Clock,
    }

    public TimerBehaviour m_Behaviour;
    public GameObject m_GameObject;
    private SaveMode m_SaveMode;
    public Action<float> m_Function;
    public bool IsDestroy { get; private set; }
    public bool IsEnabled { get; private set; }

    private TimerMode m_TimerMode;
    private bool m_Once;
    private float m_IntervalFrame;
    private float m_DeltaFrame;
    private float m_IntervalClock;
    private float m_DeltaTime;

    public TimerBehaviour behaviour { get { return m_Behaviour; } }
    public GameObject gameObject { get { return m_GameObject; } }
    public Action<float> function { get { return m_Function; } }

    public void Clear()
    {
        m_Behaviour = null;
        m_GameObject = null;
        m_Function = null;
        IsDestroy = false;
        m_IntervalFrame = 0;
        m_DeltaFrame = 0;
        m_IntervalClock = 0;
        m_DeltaTime = 0;
    }

    public void Init(TimerBehaviour bh, Action<float> action)
    {
        m_Behaviour = bh;
        if (m_Behaviour == null)
        {
            Debugger.LogError("严重错误初始化空...");
        }
        m_Function = action;
        m_SaveMode = SaveMode.Behaviour;
    }

    public void Init(GameObject go, Action<float> action)
    {
        m_GameObject = go;
        m_Function = action;
        m_SaveMode = SaveMode.GameObject;
    }

    public TimerImplement InitFrame(TimerBehaviour bh, Action<float> action, float intervalFrame, bool once)
    {
        Init(bh, action);
        m_IntervalFrame = intervalFrame;
        m_TimerMode = TimerMode.Frame;
        m_Once = once;
        return this;
    }

    public TimerImplement InitClock(TimerBehaviour bh, Action<float> action, float intervalClock, bool once)
    {
        Init(bh, action);
        m_IntervalClock = intervalClock;
        m_TimerMode = TimerMode.Clock;
        m_Once = once;
        return this;
    }

    public void Excute(float dt)
    {
        CheckState();
        if (IsDestroy || !IsEnabled)
        {
            return;
        }
        m_DeltaTime += dt;
        switch (m_TimerMode)
        {
            case TimerMode.Frame:
                m_DeltaFrame++;
                if (m_DeltaFrame >= m_IntervalFrame)
                {
                    m_Function(m_DeltaTime);
                    m_DeltaFrame = 0;
                    m_DeltaTime = 0;
                    if (m_Once)
                    {
                        SetDestroy();
                    }
                }
                break;
            case TimerMode.Clock:
                if (m_DeltaTime >= m_IntervalClock)
                {
                    m_Function(m_DeltaTime);
                    m_DeltaTime -= m_IntervalClock;
                    if (m_Once)
                    {
                        SetDestroy();
                    }
                }
                break;
        }
    }

    public void CheckState()
    {
        switch (m_SaveMode)
        {
            case SaveMode.Behaviour:
                if (m_Behaviour.IsDestroy)
                {
                    SetDestroy();
                    return;
                }
                IsEnabled = m_Once || m_Behaviour.IsEnable;
                break;
            case SaveMode.GameObject:
                if (m_GameObject == null)
                {
                    SetDestroy();
                    return;
                }
                IsEnabled = m_Once || gameObject.activeInHierarchy;
                break;
        }
    }

    public void Dispose()
    {
        Clear();
    }

    public void SetDestroy()
    {
        IsDestroy = true;
    }
}

