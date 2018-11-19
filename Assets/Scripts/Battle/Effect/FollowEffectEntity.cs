using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowEffectParam:EffectParam
{
    public float speed;

    public Vector3 offsetTo;
}

public class FollowEffectEntity : EffectEntity
{
    private float mSpeed;

    private Vector3 mOffsetTo;


    public override void Init(HeroAction _skill, EffectParam _data,HeroEntity _parentTarget)
    {
        base.Init(_skill,_data, _parentTarget);
        FollowEffectParam move = param as FollowEffectParam;
        mSpeed = move.speed;

        mOffsetTo = move.offsetTo;
        OnBegin();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        
        if(parentTarget ==null)
        {
            OnEnd();
            return;
        }
        Vector3 from = gameObject.transform.position;
      
        Vector3 to = parentTarget.position ;
        to += parentTarget.forward * mOffsetTo.z;
        to += parentTarget.right * mOffsetTo.z;
        to.y += mOffsetTo.y;

        Vector3 direction = (to - from);
        //这一帧需要的位移
        float displacement = mSpeed * deltaTime;
        if (direction.magnitude < displacement)
        {
            gameObject.transform.position = to;

            OnTrigger();
            OnEnd();
        }
        else
        {
            gameObject.transform.rotation = Quaternion.LookRotation(direction);
            gameObject.transform.position += direction.normalized * displacement;
        }
        
    }

    protected override void OnBegin()
    {
        base.OnBegin();
       
    }

    protected override void OnTrigger()
    {
        if(action.skill!=null)
        {
            action.skill.OnTrigger(parentTarget);
        }
        base.OnTrigger();
    }

    protected override void OnEnd()
    {
        base.OnEnd();
    }
}

