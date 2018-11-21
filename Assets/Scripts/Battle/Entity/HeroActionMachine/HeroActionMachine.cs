using System;
using System.Collections.Generic;

public class HeroActionMachine : StateMachine,IPool
{
    public bool isPool { get; set; }

    public HeroEntity entity { get; private set; }

    private Dictionary<ActionEnum, HeroAction> mActions;

    public HeroActionMachine()
    {
        mActions = new Dictionary<ActionEnum, HeroAction>();
    }

    public void Init(HeroEntity _entity)
    {
        entity = _entity;
        for (int i = 0; i < entity.param.actions.Count; ++i)
        {
            var param = entity.param.actions[i];
            HeroAction action = ObjectPool.GetInstance<HeroAction>();
            action.SetStateMachine(this);
            action.Init(_entity, param);
            mActions.Add(param.type, action);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        HeroAction action = GetCurrentState() as HeroAction;
        if(action ==null || action.IsPlaying()==false)
        {
            ChangeState(ActionEnum.free);
        }
        base.OnUpdate(deltaTime);

    }

    public bool IsPlaying(ActionEnum type)
    {
        var action = GetCurrentState() as HeroAction;

        if( action != null && action.type == type)
        {
            return action.IsPlaying();
        }
        return false;
    }

    public bool IsActionPlayed(ActionEnum type)
    {
        var action = GetCurrentState() as HeroAction;

        if (action != null && action.type == type)
        {
            return action.played;
        }

        return false;
    }

    public bool ChangeState(ActionEnum type)
    {
        if(IsPlaying(type))
        {
            return false;
        }

        var action = GetAction(type);
        if(action!=null)
        {
            return ChangeState(action);
        }
        return false;
    }

    public HeroAction GetAction(ActionEnum type)
    {
        if(mActions.ContainsKey(type))
        {
            return mActions[type];
        }
        return null;
    }

    public void OnCreate()
    {
       
    }

    public void OnRecycle()
    {
        var it = mActions.GetEnumerator();
        while(it.MoveNext())
        {
            ObjectPool.RecycleInstance(it.Current.Value);
        }
        mActions.Clear();
    }

    public void OnDestroy()
    {
        
    }
}

