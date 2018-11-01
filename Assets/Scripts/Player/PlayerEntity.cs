using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTree;

public class PlayerInputData:BTInput
{
    public PlayerEntity player;

    public PlayerInputData(PlayerEntity _player)
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
        mRoot = new BTRoot();

        mRoot.InitXML(data.config);
    }
	

    public void Tick(float deltaTime)
    {
        if (mInput == null)
        {
            mInput = new PlayerInputData(this);
        }
        mRoot.Tick(ref mInput);

        if (data.animationTime >= 0)
        {
            data.animationTime -= deltaTime;
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
    public void PlayAnimation(PlayerAnimationType animation,bool loop)
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
                mAnimation.wrapMode = loop ? WrapMode.Loop : WrapMode.Default;
                data.animationTime = data.animationsLength[animation];
            }
        }
    }

    public void ReleaseSkill(PlayerAnimationType animation)
    {
        data.changeType = animation;
        
    }

    public bool IsReleaseSkill()
    {
        var animationClip = data.animationType;
        if (animationClip == PlayerAnimationType.attack1
            || animationClip == PlayerAnimationType.attack2
            || animationClip == PlayerAnimationType.spell1
            || animationClip == PlayerAnimationType.spell3)
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

        return animationClip == PlayerAnimationType.run;
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

    public void Stop()
    {
        if(navMeshAgent&&!navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = true;
        }
    }

    public void Resume()
    {
        if (navMeshAgent && navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }
    }
}
