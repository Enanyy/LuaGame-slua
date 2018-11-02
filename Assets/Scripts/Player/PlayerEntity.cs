using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTree;

public class PlayerInputData:BTInput
{
    public PlayerEntity player;

    public void SetData(PlayerEntity _player)
    {
        player = _player;
    }
}

public class PlayerEntity : EntityBase<PlayerData>
{

    public NavMeshAgent navMeshAgent { get; private set; }
    private Animation mAnimation;

    private BTRoot mRoot;

   

    private BTInput mInput;

    void Awake()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        navMeshAgent.acceleration = 1000;
        navMeshAgent.angularSpeed = 7200;
        navMeshAgent.speed = 6;
        mAnimation = GetComponent<Animation>();
    }

    public override void SetData(PlayerData _data)
    {
        base.SetData(_data);

        if (string.IsNullOrEmpty(data.config) == false)
        {
            mRoot = new BTRoot();

            mRoot.InitXML(PlayerConfig.ReadXML( data.config));
        }
    }
	

    public void Tick(float deltaTime)
    {
        if (mRoot != null)
        {
            if (mInput == null)
            {
                mInput = new PlayerInputData();
                (mInput as PlayerInputData).SetData(this);
            }
            mRoot.Tick(ref mInput);
        }

        if (data.animationTime > 0)
        {
            data.animationTime -= deltaTime;
            if (data.animationTime < 0) data.animationTime = 0;
            float time = PlayerAnimations.GetAnimationLength(data.model, data.animationType);
            if(time >0)
            {
                OnAnimation(1- data.animationTime / time);
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 5, NavMesh.AllAreas))
        {
            data.destination = hit.position;
            data.changeType = PlayerAnimationType.none;        
        }
    }

    public void MoveToPoint(Vector3 destination)
    {
        data.stopDistance = PlayerData.DEFAULT_STOPDISTANCE;
        SetDestination(destination);
    }

    private float mFadeBeginTime;
    public void PlayAnimation(PlayerAnimationType animation, WrapMode mode)
    {
        bool transition = false;
        if (data.animationType != animation)
        {
            data.animationType = animation;
            mFadeBeginTime = Time.time;
            transition = true;
        }
        if (Time.time - mFadeBeginTime >= 0.2f || transition)
        {
            if (mAnimation.IsPlaying(animation.ToString()) == false)
            {
                mAnimation.CrossFade(animation.ToString(),0.2f);
                mAnimation.wrapMode = mode;
                data.animationTime = PlayerAnimations.GetAnimationLength(data.model,animation);
            }
        }
    }
    private void OnAnimation(float factor)
    {
        if (factor == 1 && IsSkill(data.animationType))
        {
            var target= PlayerManager.GetSingleton().GetPlayer(data.target);
            if(target)
            {
                target.data.hp -= 1;
            }
        }
    }

    public void ReleaseSkill(PlayerAnimationType animation)
    {
        data.changeType = animation;
        data.target = 2;
    }

    public bool IsSkill(PlayerAnimationType type)
    {
        return type == PlayerAnimationType.attack
            || type == PlayerAnimationType.attack2
            || type == PlayerAnimationType.skill
            || type == PlayerAnimationType.skill2;

    }

    public bool IsReleaseSkill()
    {
        var animationClip = data.animationType;
        if (IsSkill(animationClip))
        {
            if (data.animationTime > 0)
            {
                return true;
            }
        }


        return false;
    }

    public bool IsMoving()
    {
        var animationClip = data.animationType;

        return animationClip == PlayerAnimationType.walk;
    }

    public bool IsArrivedDestination()
    {
        Vector3 position = transform.position;
        position.y = data.destination.y;
        float distance = Vector3.Distance(data.destination, position);
        if (distance <= data.stopDistance)
        {
            return true;
        }
        return false;
    }
    public bool HasChangeSkill()
    {
        return data.changeType != PlayerAnimationType.none;
    }

    public bool HasTarget()
    {
        return data.target > 0;
    }

    public bool IsDead()
    {
        return data.hp <= 0;
    }


    public bool FindTarget()
    {
        var players = PlayerManager.GetSingleton().players;
        float minDistance = float.MaxValue;
        int minTarget = -1;
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].data.id == data.target && players[i].IsDead())
            {
                minTarget = 0;
            }
            else
            {
                if (players[i].data.camp != data.camp && players[i].IsDead() == false)
                {
                    if (minTarget == -1)
                    {
                        minTarget = players[i].data.id;
                        minDistance = Vector3.Distance(players[i].transform.position, transform.position);
                    }
                    else
                    {
                        float distance = Vector3.Distance(players[i].transform.position, transform.position);
                        if (distance < minDistance)
                        {
                            minTarget = players[i].data.id;
                            minDistance = distance;
                        }
                    }
                }
            }
        }
        if (minTarget != -1)
        {
            data.target = minTarget;
            return true;
        }

        return false;
    }

    public void LookAtTarget()
    {
        if(data.target >0)
        {
            var target = PlayerManager.GetSingleton().GetPlayer(data.target);
            if(target)
            {
                transform.LookAt(target.transform);
            }
        }
    }

    public void Stop()
    {
        if(navMeshAgent&& navMeshAgent.isOnNavMesh&& !navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = true;
        }
    }

    public void Resume()
    {
        if (navMeshAgent && navMeshAgent.isOnNavMesh && navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }
    }
}
