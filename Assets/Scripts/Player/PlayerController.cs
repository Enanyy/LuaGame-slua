using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTree;
public class PlayerInputData:BTInput
{
    public PlayerController player;

    public PlayerInputData(PlayerController _player)
    {
        player = _player;
    }
}

public class PlayerController : MonoBehaviour {

    public NavMeshAgent navMeshAgent { get; private set; }
    private Animation mAnimation;

    private BTRoot mRoot;

    public PlayerData playerData { get; private set; }

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
        mRoot = new BTRoot();
    }

    public void Init(PlayerData data)
    {
        playerData = data;
        mRoot.InitXML(playerData.config);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(mInput == null)
        {
            mInput = new PlayerInputData(this);
        }
        mRoot.Tick(ref mInput);

        if(playerData.animationTime >=0)
        {
            playerData.animationTime -= Time.deltaTime;
        }
	}

    public void MoveToPoint(Vector3 targetPosition)
    {
        playerData.destination = targetPosition;
        playerData.changeType = PlayerAnimationType.none;
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(targetPosition);
        }
        Debugger.Log("Move To Target:"+targetPosition);
    }

    private float mFadeTime;
    public void PlayAnimation(PlayerAnimationType animation,bool loop)
    {
        bool transition = false;
        if (playerData.animationType != animation)
        {
            playerData.animationType = animation;
            mFadeTime = Time.time;
            transition = true;
        }
        if (Time.time - mFadeTime >= 0.2f || transition)
        {
            if (mAnimation.IsPlaying(animation.ToString()) == false)
            {
                mAnimation.CrossFade(animation.ToString(),0.2f);
                mAnimation.wrapMode = loop ? WrapMode.Loop : WrapMode.Default;
                playerData.animationTime = playerData.animationsLength[animation];
            }
        }
    }

    public void ReleaseSkill(PlayerAnimationType animation)
    {
        playerData.changeType = animation;
        /*
        if(IsReleaseSkill())
        {
            return;
        }

        PlayAnimation(animation, false);
        Stop();
        */
    }

    public bool IsReleaseSkill()
    {
        var animationClip = playerData.animationType;
        if (animationClip == PlayerAnimationType.attack1
            || animationClip == PlayerAnimationType.attack2
            || animationClip == PlayerAnimationType.spell1
            || animationClip == PlayerAnimationType.spell3)
        {
            if (playerData.animationTime > 0)
            {
                return true;
            }
        }


        return false;
    }
    public bool HasChangeSkill()
    {
        return playerData.changeType != PlayerAnimationType.none;
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
