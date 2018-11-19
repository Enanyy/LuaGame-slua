using BTree;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonComponentParam:ComponentParam
{
    public float rotateSpeed;
}

public class CannonComponent : AIInputComponent
{
    private Quaternion mRotationTo;
    private Quaternion mRotationFrom;
    private float mRotateSpeed = 360f;
    private float mRotationTime;
    private float mRotateTime = 0;

    public override void Init(HeroEntity _entity, ComponentParam _param)
    {
        base.Init(_entity, _param);
        CannonComponentParam cannonParam = param as CannonComponentParam;
        mRotateSpeed = cannonParam.rotateSpeed;
    }

    public bool IsTurning()
    {
        return mRotateTime > 0;
    }

    void RotateToTarget()
    {
        if (entity.HasTarget() == false)
        {
            return;
        }
        if (entity. IsDead())
        {
            return;
        }
        var target = BattleManager.instance.GetEntity(entity.data.target);
        if (target != null)
        {
            Vector3 targetPosition = target.position;

            targetPosition.y = entity.position.y;
            Vector3 direction = targetPosition - entity.position;


            if (direction != Vector3.zero)
            {
                mRotationTo = Quaternion.LookRotation(direction);
                mRotationFrom = entity.rotation;

                //计算角度
                float tmpAngle = Mathf.Acos(Vector3.Dot(entity.forward, direction.normalized)) * Mathf.Rad2Deg;

                mRotationTime = tmpAngle / mRotateSpeed;
                mRotateTime = mRotationTime;
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        RatatingToTarget(deltaTime);
    }

    private void RatatingToTarget(float deltaTime)
    {
        if (mRotateTime > 0 && mRotationTime != 0)
        {
            mRotateTime -= deltaTime;
            if (mRotateTime < 0) mRotateTime = 0;
            entity.rotation = Quaternion.Lerp(mRotationFrom, mRotationTo, (mRotationTime - mRotateTime) / mRotationTime);
        }
    }

    public override bool CheckCondition(AIConditionType type)
    {
        bool result = base.CheckCondition(type);
        switch (type)
        {
            case AIConditionType.IsTurning: result = IsTurning(); break;
        }
        //Debug.Log(GetType().Name + " " +data.id+ " " + type.ToString() +" "+result);

        return result;
    }
    public override BTResult DoAction(AIActionType type)
    {
        base.DoAction(type);
        switch (type)
        {
            case AIActionType.Idle:
                {
                    RotateToTarget();
                }
                break;
            case AIActionType.FindTarget:
                {
                    RotateToTarget();
                }
                break;
        }
        //Debug.Log(GetType().Name + " "+data.id + " " + type.ToString());
        return BTResult.Success;
    }

}

