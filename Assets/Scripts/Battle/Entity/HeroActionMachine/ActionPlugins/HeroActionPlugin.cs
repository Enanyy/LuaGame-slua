using System;
using System.Collections.Generic;

public class HeroActionPlugin : State,IPoolObject
{
    public bool isPool { get; set; }

    public HeroAction action { get; private set; }
    public ActionPluginParam param { get; private set; }
    public virtual void Init(HeroAction _action, ActionPluginParam _param)
    {
        action = _action;
        param = _param;
    }

    public virtual void OnCreate()
    {
       
    }

    public virtual void OnDestroy()
    {
       
    }

    public virtual void OnReturn()
    {
       
    }
}

