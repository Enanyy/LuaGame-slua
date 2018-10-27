using System;
using UnityEngine;
using System.Collections.Generic;
public class PlayerData
{
    public string config;
    public Vector3 destination;
    public string animationClip = "idle";//当前播放的动作
    public Dictionary<string, float> animationLengths;
    public float animationTime;
}

