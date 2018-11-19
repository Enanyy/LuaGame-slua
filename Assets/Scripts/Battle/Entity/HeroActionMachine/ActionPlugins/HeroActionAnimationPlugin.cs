using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroActionAnimationPluginParam:ActionPluginParam
{
    public string animationClip;
    public WrapMode wrapMode;
}

/// <summary>
/// 播放动画组件
/// </summary>
public class HeroActionAnimationPlugin : HeroActionPlugin
{
    public const float FADELENGTH = 0.2f;

    private string mAnimationClip;
    private WrapMode mWrapMode;

    public override void Init(HeroAction _action, ActionPluginParam _param)
    {
        base.Init(_action, _param);

        var animationParam = param as HeroActionAnimationPluginParam;
        mAnimationClip = animationParam.animationClip;
        mWrapMode = animationParam.wrapMode;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        PlayAnimation();
    }

    private void PlayAnimation()
    {
        if (action.entity.animation == null || param == null)
        {
            return;
        }
       
        if (action.crossFaded == false)
        {
            var animationClip = action.entity.animation.GetClip(mAnimationClip);
            if (animationClip && action.entity.animation.IsPlaying(mAnimationClip) == false)
            {
                action.entity.animation.CrossFade(mAnimationClip, FADELENGTH);

                action.entity.animation.wrapMode = mWrapMode;

                action.crossFaded = true;

                //if (entity.data.IID <= 4)
                //    TRACE.Log(entity.data.id + ": CrossFade " + crossFaded +" name:"+name);
            }
        }

        if (action.played == false)
        {
            if (action.entity.animation.IsPlaying(mAnimationClip))
            {
                action.played = true;
                action.crossFaded = true;
            }
        }
        if (action.played)
        {
            var animationClip = action.entity.animation.GetClip(mAnimationClip);
            if (animationClip && action.entity.animation.IsPlaying(mAnimationClip) == false)
            {
                action.entity.animation.Play(mAnimationClip);
            }
        }

    }
}

