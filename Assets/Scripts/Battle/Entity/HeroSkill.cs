using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroSkill : IPool
{
    public bool isPool { get; set; }

    public SkillEnum type { get; private set; }


    public SkillParam param { get; private set; }
    //本体
    public HeroEntity entity { get; private set; }

    private float mPreFactor;
    private float mLastTime;
    public void Init(HeroEntity _entity, SkillParam _param)
    {
        entity = _entity;
        param = _param;

    }

    /// <summary>
    /// 是否CD完成
    /// </summary>
    /// <returns></returns>
    public bool IsReleasable()
    {
        return (Time.time - mLastTime) > param.cd;
    }

    /// <summary>
    /// 是否需要目标才能释放
    /// </summary>
    /// <returns></returns>
    public bool IsNeedTarget()
    {
        return param.needTarget;
    }
    /// <summary>
    /// 技能触发伤害
    /// </summary>
    public void OnTrigger(HeroEntity target)
    {
        if(target!=null && target.IsDead()==false)
        {

            target.data.hp -= (entity.data.attack -target.data.defense);

        }

    }

    public void CheckTrigger(float factor)
    {
        if (param.triggerAt >= 0)
        {
            if (mPreFactor < param.triggerAt && factor >= param.triggerAt)
            {
                var target = BattleManager.instance.GetEntity(entity.data.target);

                OnTrigger(target);
            }
            mPreFactor = factor;
        }

        //动作完成
        if (factor >= 1)
        {
            mLastTime = Time.time;
        }
    }

    public void OnCreate()
    {

    }

   

    public void OnDestroy()
    {

    }

    public void OnRecycle()
    {
        
    }
}

