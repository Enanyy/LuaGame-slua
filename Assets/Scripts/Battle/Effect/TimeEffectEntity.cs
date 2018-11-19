using System;
using System.Collections.Generic;
using UnityEngine;
public class TimeEffectParam:EffectParam
{
    public float duration;
    public float triggerAt;
}

public class TimeEffectEntity : EffectEntity
{
    private float mDuration = 0;
    private float mEffectTime;
    private float mTriggerAt = -1;
    public override void Init(HeroAction _skill, EffectParam _data, HeroEntity _parentTarget)
    {
        base.Init(_skill, _data, _parentTarget);
        var t = param as TimeEffectParam;
        mEffectTime = t.duration;
        mDuration = t.duration;
        if (t.triggerAt > 0)
        {
            mTriggerAt = t.triggerAt;
        }

        if (mEffectTime <= 0) mEffectTime = 0.1f;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (mEffectTime > 0)
        {
            float preEffectTime = mEffectTime;

            mEffectTime -= deltaTime;
            if (mEffectTime < 0) mEffectTime = 0;

            if (mTriggerAt > 0 && mDuration != 0)
            {
                float preFactor = 1 - preEffectTime / mDuration;
                float currentFactor = 1 - mEffectTime / mDuration;
                if (preFactor < mTriggerAt && currentFactor > mTriggerAt)
                {
                    OnTrigger();
                }
            }

            if (mEffectTime == 0)
            {
                OnEnd();
            }
        }
    }

    protected override void OnTrigger()
    {
        base.OnTrigger();
        if (mTriggerAt > 0 && action.skill != null)
        {
            action.skill.OnTrigger(parentTarget);
        }
    }
}

