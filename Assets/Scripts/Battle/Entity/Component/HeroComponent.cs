using System;

public abstract class HeroComponent:IPoolObject
{
    public HeroEntity entity { get; private set; }
    public ComponentParam param { get; private set; }
    public bool isPool { get ; set; }

    public virtual  void Init(HeroEntity _entity,ComponentParam _param)
    {
        entity = _entity;
        param = _param;
    }
    public virtual void OnUpdate(float deltaTime)
    {

    }

    public void OnCreate()
    {
        
    }

    public void OnDestroy()
    {
        
    }

    public void OnReturn()
    {
        
    }
}

