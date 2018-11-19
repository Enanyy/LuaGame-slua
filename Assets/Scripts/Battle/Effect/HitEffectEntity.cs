using System;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectParam:TimeEffectParam
{
    public Color hitColor;
}

public class HitEffectEntity : TimeEffectEntity
{
    private Color mOldColor;
    private MeshRenderer mRenderer;

    public override void Init(HeroAction _skill, EffectParam _data, HeroEntity _parentTarget)
    {
        base.Init(_skill, _data, _parentTarget);

        if(parentTarget!=null)
        {
            mRenderer = parentTarget.gameObject.GetComponentInChildren<MeshRenderer>();
            if(mRenderer!=null)
            {
                mOldColor = mRenderer.material.color;
                mRenderer.material.color = (param as HitEffectParam).hitColor;
            }
        }
    }
    protected override void OnEnd()
    {
        base.OnEnd();
        if (mRenderer != null)
        {
            mRenderer.material.color = mOldColor;
        }
    }
}

