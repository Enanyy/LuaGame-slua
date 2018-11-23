using System;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public sealed class HeroEntity : AssetEntity
{
    public HeroData data { get; private set; }
    public List<HeroComponent> components { get; private set; }
    public Dictionary<SkillEnum, HeroSkill> skills { get; private set; }
    public HeroActionMachine machine { get; private set; }
    public HeroParam param { get; private set; }
    private Animation mAnimation;
    public Animation animation
    {
        get
        {
            if (mAnimation == null)
            {
                if (gameObject != null)
                {
                    mAnimation = gameObject.GetComponent<Animation>();
                    if (mAnimation == null)
                    {
                        mAnimation = gameObject.GetComponentInChildren<Animation>();
                    }
                }
            }
            return mAnimation;
        }
    }
    public Vector3 position { get { return gameObject.transform.position; } set { SetPosition(value); } }
    public Quaternion rotation { get { return gameObject.transform.rotation; } set { gameObject.transform.rotation = value; } }
    public Vector3 forward { get { return gameObject.transform.forward; } set { gameObject.transform.forward = value; } }
    public Vector3 right { get { return gameObject.transform.right; } set { gameObject.transform.right = value; } }
    public HeroEntity()
    {
        skills = new Dictionary<SkillEnum, HeroSkill>();
        components = new List<HeroComponent>();
    }

    public  void Init(HeroData _data)
    {
        data = _data;

        param = null;

        SetPosition(new Vector3(data.x, data.y, data.z));
        SetRotation(new Vector3(0, data.rotation, 0));

        skills.Clear();

        HeroConfig.GetPlayerConfig(data.config, (HeroParam config) => {

            if (config != null)
            {
                OnInitParam(config);
            }
            else
            {
                Debug.LogError("Can't find config:" + data.config);
            }
        });
    } 
    private  void OnInitParam(HeroParam _param)
    {
        param = _param;
        if(string.IsNullOrEmpty(data.name))
        {
            data.name = param.name;
        }
        if(data.height == 0)
        {
            data.height = param.height;
        }
  

        if(param.components!=null)
        {
            for(int i = 0; i< param.components.Count;++i)
            {
                HeroComponent component = ObjectPool.GetInstance<HeroComponent>(param.components[i].type);
                component.Init(this, param.components[i]);
                components.Add(component);
            }
        }

        //初始化技能
        if (param.skills != null)
        {
            for (int i = 0; i < param.skills.Count; ++i)
            {
                var skill = ObjectPool.GetInstance<HeroSkill>();
                skill.Init(this, param.skills[i]);
                skills.Add(param.skills[i].type, skill);
            }
        }
        //初始化动作状态机
        machine = ObjectPool.GetInstance<HeroActionMachine>();
        machine.Init(this);

        string assetBundleName = string.Format("assets/r/{0}.prefab", param.assetBundleName);
        string assetName = string.Format("assets/r/{0}.prefab", param.assetName);

        LoadAsset(assetBundleName, assetName);

    }
    protected override void OnLoadAsset(GameObject go)
    {
        go.transform.localScale = Vector3.one * data.scale;
    }

    public override void Recycle()
    {
        var it = skills.GetEnumerator();
        while(it.MoveNext())
        {
            ObjectPool.RecycleInstance(it.Current.Value);
        }
        skills.Clear();
        ObjectPool.RecycleInstance(data);

        base.Recycle();
       
        machine.Clear();
        ObjectPool.RecycleInstance(machine);
        mAnimation = null;

        for(int i = 0; i < components.Count; ++ i)
        {
            ObjectPool.RecycleInstance(components[i], components[i].GetType());
        }
        components.Clear();
    }
    public bool ReleaseSkill(SkillEnum skillType)
    {
        if (IsAttacking())
        {
            return false;
        }


        var skill = GetSkill(skillType);
        return ReleaseSkill(skill);
    }

    

    public  bool ReleaseSkill(HeroSkill skill)
    {
        if (skill == null || skill.IsReleasable() == false)
        {
            return false;
        }
        if (skill.IsNeedTarget() && HasTarget() == false)
        {
            return false;
        }

        var action = ActionEnum.none;
        if (skill.param != null)
        {
            action = skill.param.action;
        }
        LookAtTarget();

        bool result= PlayAction(action);


        return result;
    }

    public bool PlayAction(ActionEnum action)
    {
       
        if(machine!=null)
        {
           return machine.ChangeState(action);
        }
        return false;
    }



    public  void LookAtTarget()
    {
        if (data.target != 0)
        {
            var target = BattleManager.instance.GetEntity(data.target);
            if (target != null && target.gameObject)
            {
                gameObject.transform.LookAt(target.gameObject.transform);
            }
        }
    }
    public bool HasTarget()
    {
        bool result = false;
        if (data.target != 0)
        {
            var target = BattleManager.instance.GetEntity(data.target);
            if (target != null && target.data.camp != data.camp && target.IsDead() == false)
            {
                float distance = Vector3.Distance(target.position, position);
                if(distance < data.searchDistance)
                {
                    return true;
                }
            }
            
        }
        if(result == false)
        {
            data.target = 0;
        }
        return result;
    }

    public bool IsDead()
    {
        return data.hp <= 0;
    }
    public  bool IsAttacking()
    {
        var it = skills.GetEnumerator();
        while(it.MoveNext())
        {
            if(machine!=null && machine.IsPlaying(it.Current.Value.param.action))
            {
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// 是否有技能可释放
    /// </summary>
    /// <returns></returns>
    public  bool HasAttack()
    {
        var it = skills.GetEnumerator();
        while (it.MoveNext())
        {
            if ( it.Current.Value.IsReleasable())
            {
                return true;
            }
        }

        return false;
    }

    public  bool IsAttackRange()
    {
        if(HasTarget()==false)
        {
            return false;
        }
        HeroEntity target = BattleManager.instance.GetEntity(data.target);
        if(target!=null)
        {
            float distance = Vector3.Distance(position, target.position);
            if(distance < data.attackDistance)
            {
                return true;
            }
        }
        return false;
    }

    public HeroSkill GetReleasableSkill(SkillType type)
    {
        var it = skills.GetEnumerator();
        while (it.MoveNext())
        {
            if (it.Current.Value.IsReleasable() && it.Current.Value.param.skillType == type)
            {
                return it.Current.Value;
            }
        }
        return null;
    }

    public HeroSkill GetSkill(SkillEnum type)
    {
        if(skills.ContainsKey(type))
        {
            return skills[type];
        }
        return null;
    }

    public HeroSkill GetSkill(ActionEnum action)
    {
        var it = skills.GetEnumerator();
        while(it.MoveNext())
        {
            if(it.Current.Value.param.action == action)
            {
                return it.Current.Value;
            }
        }
        return null;
    }
    public HeroSkill GetSkill(SkillType type)
    {
        var it = skills.GetEnumerator();
        while (it.MoveNext())
        {
            if (it.Current.Value.param.skillType == type)
            {
                return it.Current.Value;
            }
        }
        return null;
    }


    public  void  SetPosition( Vector3 position)
    {
        gameObject.transform.position = position;
    }
    public void SetForword(Vector3 forward)
    {
        if (forward != Vector3.zero)
        {
            gameObject.transform.forward = forward;
        }
    }
    public void SetRotation(Vector3 rotation)
    {
        gameObject.transform.rotation = Quaternion.Euler(rotation);
    }
    public  bool FindTarget()
    {
        if (IsDead())
        {
            data.target = 0;

            return false;
        }

        var players = BattleManager.instance.entities;
        float minDistance = float.MaxValue;
        int minTarget = 0;
        for (int i = 0; i < players.Count; ++i)
        {
            var player = players[i];
            //自己 或者同阵营
            if(player.data.id == data.id || player.data.camp == data.camp)
            {
                continue;
            }
            float distance = Vector3.Distance(player.position, position);

            //大于视野或者已经死亡
            if(distance > data.searchDistance || player.IsDead())
            {
                if(player.data.id == data.target)
                {
                    data.target = 0;
                }
            }

            else
            {
                if (distance < data.searchDistance && player.IsDead() == false)
                {
                    if (minTarget == 0)
                    {
                        minTarget = players[i].data.id;
                        minDistance = Vector3.Distance(players[i].position, position);
                    }
                    else
                    {
                        if (distance < minDistance)
                        {
                            minTarget = players[i].data.id;
                            minDistance = distance;
                        }
                    }
                }
            }
        }

        data.lastTarget = data.target;
        data.target = minTarget;

        return minTarget != 0;
    }

    public  void OnUpdate(float deltaTime)
    {
        //先更新组件
        for(int i =0; i < components.Count; ++i)
        {
            components[i].OnUpdate(deltaTime);
        }
        //更新动作状态机
        if (machine != null)
        {
            machine.OnUpdate(deltaTime);
        }
    }
}

