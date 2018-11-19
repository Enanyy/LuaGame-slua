using System;
using UnityEngine;

public enum EffectArise
{
    ParentBegin,
    ParentTrigger,
    ParentEnd,
}

public enum EffectOn
{
    Self,//
    Target,
    Custom
}

/// <summary>
/// 特效
/// </summary>
public class EffectEntity : AssetEntity
{
    public HeroAction action { get; private set; }
    public EffectParam param { get; private set; }
    public HeroEntity parentTarget { get; private set; }

    public virtual void Init(HeroAction _action, EffectParam _data, HeroEntity _parentTarget)
    {
        action = _action;
        param = _data;
        parentTarget = _parentTarget;

        if (param.effectOn == EffectOn.Custom)
        {
        }
        else
        {
            GameObject go = action.entity.gameObject;
            if (param.effectOn == EffectOn.Self)
            {
                go = action.entity.gameObject;
            }
            else if (param.effectOn == EffectOn.Target)
            {
                if (parentTarget != null)
                {
                    go = parentTarget.gameObject;
                }
            }
            if (go)
            {
                Vector3 position = go.transform.position;
                position += go.transform.forward * param.offset.z;
                position += go.transform.right * param.offset.x;
                position.y += param.offset.y;

                gameObject.transform.position = position;
                gameObject.transform.rotation = Quaternion.LookRotation(go.transform.forward);

                string assetBundleName = string.Format("assets/assetBundle/{0}.prefab", param.assetBundleName);
                string assetName = string.Format("assets/assetBundle/{0}.prefab", param.assetName);


                LoadAsset(assetBundleName, assetName);
            }
            else
            {
                OnEnd();
            }
        }
    }



    public virtual void OnUpdate(float deltaTime)
    {

    }

    public override void Recycle()
    {
        base.Recycle();
    }

    /// <summary>
    /// 特效开始显示
    /// </summary>
    protected virtual void OnBegin()
    {
        ShowEffect(EffectArise.ParentBegin);

    }
    /// <summary>
    /// 特效出发
    /// </summary>
    protected virtual void OnTrigger()
    {
        ShowEffect(EffectArise.ParentTrigger);

    }

    /// <summary>
    /// 特效结束
    /// </summary>
    protected virtual void OnEnd()
    {
        BattleManager.instance.RemoveEffect(this);
        ShowEffect(EffectArise.ParentEnd);
    }

    /// <summary>
    /// 显示子特效
    /// </summary>
    /// <param name="effectType"></param>
    private void ShowEffect(EffectArise effectType)
    {
        if (param != null && param.effects != null)
        {
            for (int i = 0; i < param.effects.Count; ++i)
            {
                var effect = param.effects[i];
                if (effect.effectArise == effectType)
                {
                    EffectEntity entity = BattleManager.instance.CreateEffect(effect.type);
                    entity.Init(action, param.effects[i], parentTarget);
                }
            }
        }
    }
}

