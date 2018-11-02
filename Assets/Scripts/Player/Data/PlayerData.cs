using System;
using UnityEngine;
using System.Collections.Generic;
public enum PlayerAnimationType
{
    none,
    free,
    idle,
    walk,
    attack,
    attack2,
    skill,
    skill2,
    death   
}
public class PlayerData
{
    public int id;
    public int camp = 0; //阵营
    public float x;
    public float z;
    public float dirX;
    public float dirZ;

    public int target = -1;
    public string model;
    public string config;
    public Vector3 destination;
    public PlayerAnimationType animationType = PlayerAnimationType.idle;//当前播放的动作
    public PlayerAnimationType changeType = PlayerAnimationType.none;//准备要播放的动作
    public float animationTime;
    public const float DEFAULT_STOPDISTANCE = 0.01f;
    public float stopDistance = DEFAULT_STOPDISTANCE;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attackDistance = 1.5f;
    public float hp;
}

