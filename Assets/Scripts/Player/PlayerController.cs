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

    public NavMeshAgent mNavMeshAgent;
    private Animation mAnimation;

    private BTRoot mRoot;

    public PlayerData playerData { get; private set; }

    private BTInput mInput;

    void Awake()
    {
        if (mNavMeshAgent == null)
        {
            mNavMeshAgent = GetComponent<NavMeshAgent>();
        }
        if (mNavMeshAgent == null)
        {
            mNavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        mNavMeshAgent.acceleration = 1000;
        mNavMeshAgent.angularSpeed = 7200;
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
        if (mNavMeshAgent.isOnNavMesh)
        {
            mNavMeshAgent.SetDestination(targetPosition);
        }
        Debugger.Log("Move To Target:"+targetPosition);
    }

    private float mFadeTime;
    public void PlayAnimation(string animationClip,bool loop)
    {
        bool transition = false;
        if (playerData.animationClip != animationClip)
        {
            playerData.animationClip = animationClip;
            mFadeTime = Time.time;
            transition = true;
        }
        if (Time.time - mFadeTime >= 0.2f || transition)
        {
            if (mAnimation.IsPlaying(animationClip) == false)
            {
                mAnimation.CrossFade(animationClip,0.2f);
                mAnimation.wrapMode = loop ? WrapMode.Loop : WrapMode.Default;
                playerData.animationTime = playerData.animationLengths[animationClip];
            }
        }
    }

    public void ReleaseSkill(string name)
    {
        if(IsReleaseSkill())
        {
            return;
        }

        PlayAnimation(name, false);
        mNavMeshAgent.isStopped = true;
    }

    public bool IsReleaseSkill()
    {
        string animationClip = playerData.animationClip;
        if (animationClip == "attack1"
            || animationClip == "attack2"
            || animationClip == "spell1"
            || animationClip == "spell3")
        {
            if (playerData.animationTime > 0)
            {
                return true;
            }
        }


        return false;
    }
}
