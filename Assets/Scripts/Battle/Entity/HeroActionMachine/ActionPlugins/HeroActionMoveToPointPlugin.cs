using System;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
public class HeroActionMoveToPointPlugin : HeroActionPlugin
{
    private List<Vector3> mPath = new List<Vector3>();
    private int mIndex = 0;
    private float mRotateSpeed = 10;

    public override void OnEnter()
    {
        base.OnEnter();

        if(BattleManager.instance.FindPath(action.entity.position, action.entity.data.destination, ref mPath))
        {
            mIndex = 0;
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (action.crossFaded)
        {
            var player = action.entity;

            if (mPath.Count > 0)
            {
                //目标改变
                if (mPath[mPath.Count - 1] != player.data.destination)
                {
                    Vector3 lastDestination = mPath[mPath.Count - 1];
                    //上次的目标与新目标距离过大，需要重新寻路
                    if(Vector3.Distance(lastDestination, player.data.destination) >10)
                    {
                        //重新寻路
                        if (BattleManager.instance.FindPath(action.entity.position, action.entity.data.destination, ref mPath))
                        {
                            mIndex = 0;
                        }
                    }
                    else
                    {
                        mPath[mPath.Count - 1] = player.data.destination;
                    }
                }

                if (mIndex < mPath.Count)
                {
                    Vector3 corner = mPath[mIndex];
                    Vector3 position = player.position;
                    position.y = corner.y;

                    Vector3 direction = corner - position;
                    //这一帧需要的位移
                    float displacement = player.data.speed * deltaTime;

                    if (direction.magnitude > displacement)
                    {
                        player.position += direction.normalized * displacement;

                        Quaternion rotation = Quaternion.LookRotation(direction);

                        player.rotation = Quaternion.Lerp(player.rotation, rotation, mRotateSpeed * Time.deltaTime);
                    }
                    else
                    {
                        player.position = corner;
                        mIndex++;
                    }
                }

            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        mPath.Clear();
        mIndex = 0;
    }
}

