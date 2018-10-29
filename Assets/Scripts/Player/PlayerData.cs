using System;
using UnityEngine;
using System.Collections.Generic;
public enum PlayerAnimationType
{
    none,
    idle,
    run,
    attack1,
    attack2,
    spell1,
    spell2,
    spell3,
    spell4,
    dance,
    die,
    sneak   
}
public class PlayerData
{
    public string config;
    public Vector3 destination;
    public PlayerAnimationType animationType = PlayerAnimationType.idle;//当前播放的动作
    public PlayerAnimationType changeType = PlayerAnimationType.none;//下个的动作
    public Dictionary<PlayerAnimationType, float> animationsLength;
    public float animationTime;
}

