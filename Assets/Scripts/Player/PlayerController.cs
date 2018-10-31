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

    public PlayerData data { get; private set; }

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

    public void Init(PlayerData _data)
    {
        data = _data;
        mRoot.InitXML(data.config);
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

        if(data.animationTime >=0)
        {
            data.animationTime -= Time.deltaTime;
        }
	}

    public void MoveToPoint(Vector3 targetPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 5, NavMesh.AllAreas))
        {
            data.destination = hit.position;
            data.changeType = PlayerAnimationType.none;
            
        }
    }

    private float mFadeTime;
    public void PlayAnimation(PlayerAnimationType animation,bool loop)
    {
        bool transition = false;
        if (data.animationType != animation)
        {
            data.animationType = animation;
            mFadeTime = Time.time;
            transition = true;
        }
        if (Time.time - mFadeTime >= 0.2f || transition)
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
