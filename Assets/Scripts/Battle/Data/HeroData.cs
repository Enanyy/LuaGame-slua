using System;
using UnityEngine;
using System.Collections.Generic;

public enum HeroCamp
{
    Attack,
    Defense
}

public class HeroData : EntityData
{
    public int id;              //唯一ID
    public int profession;      //英雄职业 1 英雄 2 炮
    public string name;         //头顶名字
    public HeroCamp camp;       //阵营
    public int IID;            //表的ID
    public float x;
    public float y;
    public float z;
    public float rotation;
    public float scale = 1;
    public float speed;
    public int target = 0;
    public int lastTarget = 0;//上一个目标
    public string config;
    public string ai;
    public float height;//模型高度

    /// <summary>
    /// 寻找距离
    /// </summary>
    public float searchDistance;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attackDistance;
    /// <summary>
    /// 血量
    /// </summary>
    public int hp;
    /// <summary>
    /// 最大血量
    /// </summary>
    public int maxhp;

    public int attack;
    public int defense;

    /// <summary>
    /// 移动的目的地
    /// </summary>
    public Vector3 destination;

    /// <summary>
    /// 默认停止距离
    /// </summary>
    public const float DEFAULT_STOPDISTANCE = 0.01f;
    /// <summary>
    /// 停止的距离
    /// </summary>
    public float stopDistance = DEFAULT_STOPDISTANCE;

    public void Clear()
    {
        id = 0;              //唯一ID
        profession = 0;      //英雄职业 1 英雄 2 炮
        name = "";         //头顶名字
        camp = HeroCamp.Attack;       //阵营
        IID = 0;            //表的ID
        x = 0;
        y = 0;
        z = 0;
        rotation = 0;
        scale = 1;
        speed = 0;
        target = 0;
        lastTarget = 0;//上一个目标
        config = "";
        ai = "";
        height = 0;//模型高度
        searchDistance = 0;
        attackDistance = 0;
        hp = 0;
        maxhp = 0;
        attack = 0;
        defense = 0;

        destination = Vector3.zero;
        stopDistance = DEFAULT_STOPDISTANCE;
    }
}

