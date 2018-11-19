using BTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerComponentParam:ComponentParam
{
    public float speed;
}

public class PlayerComponent : AIInputComponent
{
    public bool needStop { get; private set; }
    public override void Init(HeroEntity _entity, ComponentParam _param)
    {
        base.Init(_entity, _param);



        SetDestination(entity.position);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (needStop)
        {
            if (entity.machine != null && entity.machine.IsPlaying(ActionEnum.free))
            {
                needStop = false;
            }
            else
            {
                entity.PlayAction(ActionEnum.free);
            }
        }
    }

    private void SetDestination(Vector3 position)
    {
        UnityEngine.AI.NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 5, NavMesh.AllAreas))
        {
           entity.data. destination = hit.position;
        }
    }

    public void MoveToPoint(Vector3 position)
    {
        entity.data.stopDistance = HeroData.DEFAULT_STOPDISTANCE;

        SetDestination(position);
    }


    public bool IsMoving()
    {
        if (entity.machine != null)
        {
            return entity.machine.IsPlaying(ActionEnum.walk);
        }
        return false;
    }
    /// <summary>
    /// 是否有技能可释放
    /// </summary>
    /// <returns></returns>
    public  bool HasAttack()
    {
        var it = entity.skills.GetEnumerator();
        while (it.MoveNext())
        {
            if (it.Current.Key == SkillEnum.Attack && it.Current.Value.IsReleasable())
            {
                return true;
            }
        }

        return false;
    }

    public bool IsArrive()
    {
        Vector3 position = entity.position;
        position.y = entity.data.destination.y;
        float distance = Vector3.Distance(entity.data.destination, position);
        if (distance <= entity.data.stopDistance)
        {
            return true;
        }

        return false;
    }



    private void FollowTarget()
    {
        if (entity.IsDead() == false)
        {
            if (entity.HasTarget())
            {
                if (entity.IsAttackRange() == false
                    && needStop == false)
                {
                    var target = BattleManager.instance.GetEntity(entity.data.target);
                    if (target != null)
                    {
                        Vector3 targetPosition = target.position;

                        Vector3 foundPosition = BattleManager.instance.MoveTowardsWidthUmbrella(entity, targetPosition, entity.data.attackDistance - 1, 60);
                        entity.data.stopDistance = HeroData.DEFAULT_STOPDISTANCE; //entity.data.attackDistance;
                        SetDestination(foundPosition);


                        entity.PlayAction(ActionEnum.walk);

                    }
                }
            }
            else
            {
                SetDestination(entity.position);
            }
        }
    }


    public override BTResult DoAction(AIActionType type)
    {
        base.DoAction(type);
        switch (type)
        {
            case AIActionType.MoveToPoint:
                {
                   entity.PlayAction(ActionEnum.walk);
                }
                break;
            case AIActionType.Attack:
                {
                    entity.LookAtTarget();
                   // SetDestination(entity.position);
                }
                break;
            case AIActionType.Die:
                {
                    needStop = true;

                }
                break;
            case AIActionType.FollowTarget:
                {
                    FollowTarget();
                }
                break;

        }

        //if (data.id == 2)
        //{
        //   Debug.Log(GetType().Name + " " + type.ToString());
        //}
        return BTResult.Success;
    }
    public override bool CheckCondition(AIConditionType type)
    {
        bool result = base.CheckCondition(type);
        switch (type)
        {
            case AIConditionType.HasAttack:result = HasAttack();break;
            case AIConditionType.IsArrive: result = IsArrive(); break;
            case AIConditionType.IsMoving: result = IsMoving(); break;
        }

        //if (data.id == 2)
        //{
        // Debug.Log(GetType().Name + " " + type.ToString() + " " + result);
        //}
        return result;

    }
}

