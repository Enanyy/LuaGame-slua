using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroAction : State,IPool
{
    public bool isPool { get; set; }

    public HeroAction() { }
    public HeroAction(string name) : base(name) { }

    public ActionEnum type { get { return param.type; } }

    private float mAnimationTime;
    public bool  crossFaded { get; set; }
    public bool played { get;  set; }

    public ActionParam param { get; private set; }
    //本体
    public HeroEntity entity { get; private set; }

    public HeroSkill skill { get; private set; }

    public List<HeroActionPlugin> plugins { get; private set; }
    //private float mFadeTime;

    public HeroActionMachine actionMachine { get { return machine as HeroActionMachine; } }

    public void Init(HeroEntity _entity, ActionParam _param)
    {
        entity = _entity;
        param = _param;
        mName = param.type.ToString();
        skill = entity.GetSkill(param.type);

        if(param.actions!=null && param.actions.Count >0)
        {
            plugins = new List<HeroActionPlugin>();

            for (int i = 0; i <param.actions.Count;++i)
            {
                HeroActionPlugin plugin = ObjectPool.GetInstance<HeroActionPlugin>(param.actions[i].type);
                plugin.SetStateMachine(machine);
                plugin.Init(this, param.actions[i]);
                plugins.Add(plugin);
            }
        }
    }

    /// <summary>
    /// 是否在播放动画
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        return mAnimationTime > 0;
    }

   

    public override void OnUpdate(float deltaTime)
    {
        if (mAnimationTime >0)
        {
            mAnimationTime -= deltaTime;
            if (mAnimationTime < 0) mAnimationTime = 0;

            if (skill != null && param.length != 0)
            {
                skill.CheckTrigger(1 - mAnimationTime / param.length);
            }
        }
        if(plugins!=null && plugins.Count >0)
        {
            for(int i = 0; i < plugins.Count;++i)
            {
                plugins[i].OnUpdate(deltaTime);
            }
        }
    }

    public override void OnEnter()
    {
        //if (entity.data.IID <= 4)
        //    TRACE.Log(entity.data.id + ":" + type.ToString());

        mAnimationTime = param.length;

        crossFaded = false;
        played = false;
        if (skill != null )
        {
            skill.CheckTrigger(0);
        }
        ShowEffect(EffectArise.ParentBegin);

        if (plugins != null && plugins.Count > 0)
        {
            for (int i = 0; i < plugins.Count; ++i)
            {
                plugins[i].OnEnter();
            }
        }
    }

    public override void OnExit()
    {
        if (plugins != null && plugins.Count > 0)
        {
            for (int i = 0; i < plugins.Count; ++i)
            {
                plugins[i].OnExit();
            }
        }
        mAnimationTime = 0;
        crossFaded = false;
        played = false;
        if (skill != null)
        {
            skill.CheckTrigger(1);
        }
        ShowEffect(EffectArise.ParentEnd);
    }


   

    private void ShowEffect(EffectArise effectType)
    {
        if (param != null && param.effects != null && entity.isPool == false)
        {
            for (int i = 0; i < param.effects.Count; ++i)
            {
                if (param.effects[i].effectArise == effectType)
                {

                    var target = BattleManager.instance.GetEntity(entity.data.target);

                    if(target != null && target.isPool)
                    {
                        return;
                    }

                    EffectEntity effect = BattleManager.instance.CreateEffect(param.effects[i].type);


                    effect.Init(this, param.effects[i], target);
                }
            }
        }
    }

    public void OnCreate()
    {
       
    }

    public void OnRecycle()
    {
        mAnimationTime = 0;
        crossFaded = false;
        played = false;

        if (plugins!=null)
        {
            for(int i = 0; i < plugins.Count;++i)
            {
                ObjectPool.RecycleInstance(plugins[i], plugins[i].GetType());
            }
            plugins.Clear();
        }
    }

    public void OnDestroy()
    {
        
    }
}

