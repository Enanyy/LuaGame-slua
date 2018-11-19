using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mono.Xml;
using System.Security;

public class HeroParam
{
    public string name;
    public string assetBundleName;
    public string assetName;
    public float height;
    public float radius;
    //public float searchDistance;
    //public float attackDistance;
    public List<SkillParam> skills = new List<SkillParam>();
    public List< ActionParam> actions = new List<ActionParam>();
    public List<ComponentParam> components = new List<ComponentParam>();
}

/// <summary>
/// 组件参数
/// </summary>
public class ComponentParam
{
    /// <summary>
    /// 组件类型
    /// </summary>
    public Type type;

}

/// <summary>
/// 技能参数
/// </summary>
public class SkillParam
{
    public SkillEnum type;
    public float cd;
    public float triggerAt;
    public ActionEnum action;
    public bool needTarget;
    public SkillType skillType;

    //攻击力
    //public int attack;

}

/// <summary>
/// 特效参数
/// </summary>
public class EffectParam
{
    public Type type;
    public string assetBundleName;
    public string assetName;
    public EffectArise effectArise;
    public Vector3 offset;
    public EffectOn effectOn;
    /// <summary>
    /// 子特效
    /// </summary>
    public List<EffectParam> effects;
}

/// <summary>
/// 动作参数
/// </summary>
public class ActionParam
{
    public ActionEnum type;  
    public float length;
    public List<EffectParam> effects;
    public List<ActionPluginParam> actions;
}

/// <summary>
/// 动作插件参数
/// </summary>
public class ActionPluginParam
{
    /// <summary>
    /// 插件类型
    /// </summary>
    public Type type;
}



public static class HeroConfig
{
    static Dictionary<string, HeroParam> heroParams = new Dictionary<string, HeroParam>();

    public static void GetPlayerConfig(string config,Action<HeroParam> callback)
    {
        if(heroParams.ContainsKey(config))
        {
            if(callback!=null)
            {
                callback(heroParams[config]);
            }
        }
        else
        {
            LoadConfig(config, callback);
        }
    }
    private static void LoadConfig(string name, Action<HeroParam> callback)
    {
        string assetBundleName = string.Format("assets/assetbundle/config/character/hero/{0}.xml", name.ToLower());
        AssetCache.LoadAssetAsync<TextAsset>(assetBundleName, assetBundleName, (asset) =>
        {
            if (asset != null)
            {
                TextAsset textAsset = asset.GetAsset() as TextAsset;
                string text = textAsset.text;
                if (string.IsNullOrEmpty(text) == false)
                {
                    SecurityParser parser = new SecurityParser();

                    parser.LoadXml(text);

                    SecurityElement element = parser.ToXml();

                    HeroParam param = new HeroParam();

                    ParseConfig(ref param, element);
                    if (heroParams.ContainsKey(name) == false)
                    {
                        heroParams.Add(name, param);
                    }

                    if (callback != null)
                    {
                        callback(param);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(null);
                    }
                }
            }
        });
    }

    private static void ParseConfig(ref HeroParam param, SecurityElement element)
    {
        if(param ==null|| element==null || element.Tag !="Hero" )
        {
            return;
        }
        param.name = element.Attribute("name");
        param.height = element.Attribute("height").ToFloatEx();
        param.radius = element.Attribute("radius").ToFloatEx();
        param.assetBundleName = element.Attribute("bundleName");
        param.assetName = element.Attribute("assetName");
        //param.searchDistance = element.Attribute("searchDistance").ToFloatEx();
        //param.attackDistance = element.Attribute("attackDistance").ToFloatEx();
        param.skills = new List<SkillParam>();
        param.actions = new List<ActionParam>();
        param.components = new List<ComponentParam>();
        if (element.Children != null)
        {
            for (int i = 0; i < element.Children.Count; ++i)
            {
                var child = element.Children[i] as SecurityElement;
                if (child.Tag == "Skill")
                {
                    ParseSkill(ref param, child);
                }
                else if (child.Tag == "Action")
                {
                    ParseAction(ref param.actions, child);
                }
            
                else if(child.Tag == "Component")
                {
                    ParseComponent(ref param, child);
                }
            }
        }
    }

    private static void ParseSkill(ref HeroParam param, SecurityElement element)
    {
        if (param == null || element == null || element.Tag != "Skill")
        {
            return;
        }

        SkillParam skill = new SkillParam();
        skill.type =  element.Attribute("enum").ToEnumEx<SkillEnum>();
        skill.cd = element.Attribute("cd").ToFloatEx();
        //skill.attack = element.Attribute("attack").ToInt32Ex();
        skill.action = element.Attribute("action").ToEnumEx<ActionEnum>();
        skill.triggerAt = element.Attribute("triggerAt").ToFloatEx();
        skill.needTarget = element.Attribute("needTarget").ToBoolEx();
        skill.skillType = element.Attribute("skillType").ToEnumEx<SkillType>();
        //skill.effects = new List<EffectParam>();
        param.skills.Add(skill);

    }

    private static void ParseAction(ref List<ActionParam> actions, SecurityElement element)
    {
        if (actions == null || element == null || element.Tag != "Action")
        {
            return;
        }
        ActionParam action = new ActionParam();
        action.type = element.Attribute("enum").ToEnumEx<ActionEnum>();
        action.length = element.Attribute("length").ToFloatEx();
        actions.Add(action);

        if (element.Children != null)
        {
            for (int i = 0; i < element.Children.Count; ++i)
            {
                var child = element.Children[i] as SecurityElement;
                if (child.Tag == "Effect")
                {
                    if (action.effects == null)
                    {
                        action.effects = new List<EffectParam>();
                    }
                    ParseEffect(ref action.effects, child);
                }
                else if(child.Tag == "Plugin")
                {
                    if(action.actions == null)
                    {
                        action.actions = new List<ActionPluginParam>();
                    }
                    ParseActionPlugin(ref action.actions, child);
                }
            }
        }
    }

    private static void ParseActionPlugin(ref List<ActionPluginParam> plugins, SecurityElement element)
    {
        if (element == null || element.Tag != "Plugin")
        {
            return;
        }
        ActionPluginParam plugin= (ActionPluginParam)Activator.CreateInstance(Type.GetType(element.Attribute("param")));
        plugin.type = Type.GetType( element.Attribute("type"));

        plugins.Add(plugin);

        if (element.Children != null)
        {
            for (int i = 0; i < element.Children.Count; ++i)
            {
                var child = element.Children[i] as SecurityElement;
                if (child.Tag == "Param")
                {
                    ParseActionPluginParam(ref plugin, child);
                }
            }
        }
    }

    private static void ParseActionPluginParam(ref ActionPluginParam param, SecurityElement element)
    {
        if (param == null || element == null || element.Tag != "Param")
        {
            return;
        }
        if (param.type == typeof(HeroActionAnimationPlugin))
        {
            var animation = param as HeroActionAnimationPluginParam;
            animation.animationClip = element.Attribute("animationClip");
            animation.wrapMode = element.Attribute("wrapMode").ToEnumEx<WrapMode>();
        }
    }




    private static void ParseComponent(ref HeroParam param, SecurityElement element)
    {
        if (element == null || element.Tag != "Component")
        {
            return;
        }
        ComponentParam component = (ComponentParam)Activator.CreateInstance(Type.GetType(element.Attribute("param")));
        component.type = Type.GetType(element.Attribute("type"));

        param.components.Add(component);
        
        if(element.Children !=null )
        {
            for(int i = 0; i < element.Children.Count; ++i)
            {
                var child = element.Children[i] as SecurityElement;
                if(child.Tag == "Param")
                {
                    ParseComponentParam(ref component, child);
                }
            }
        }
    }

    private static void ParseComponentParam(ref ComponentParam param, SecurityElement element)
    {
        if (param == null || element == null || element.Tag != "Param")
        {
            return;
        }
        if (param.type == typeof(CannonComponent))
        {
            var move = param as CannonComponentParam;
            move.rotateSpeed = element.Attribute("rotateSpeed").ToFloatEx();
        }
        else if(param.type == typeof(PlayerComponent))
        {
            var move = param as PlayerComponentParam;
            move.speed = element.Attribute("speed").ToFloatEx();
        }
    }


    private static void ParseEffect(ref List<EffectParam> effects, SecurityElement element)
    {
        if (element == null || element.Tag != "Effect")
        {
            return;
        }

        EffectParam effect = (EffectParam)Activator.CreateInstance(Type.GetType(element.Attribute("param")));

        effect.type = Type.GetType(element.Attribute("type"));
        effect.assetBundleName = element.Attribute("bundleName");
        effect.assetName = element.Attribute("assetName");
        effect.effectArise = element.Attribute("effectArise").ToEnumEx<EffectArise>();
        effect.effectOn = element.Attribute("effectOn").ToEnumEx<EffectOn>();
        effect.offset = element.Attribute("offset").ToVector3Ex();
        effects.Add(effect);

        if (element.Children != null)
        {
            for (int i = 0; i < element.Children.Count; ++i)
            {
                var child = element.Children[i] as SecurityElement;
                if (child.Tag == "Effect")
                {
                    if(effect.effects==null)
                    {
                        effect.effects = new List<EffectParam>();
                    }
                    ParseEffect(ref effect.effects, child);
                }
                else if (child.Tag == "Param")
                {
                    ParseEffectParam(ref effect, child);
                }
            }
        }
    }
 

    private static void ParseEffectParam(ref EffectParam param, SecurityElement element)
    {
        if (param == null || element == null || element.Tag != "Param")
        {
            return;
        }
        if (param.type == typeof(FollowEffectEntity))
        {
            var move = param as FollowEffectParam;
            move.speed = element.Attribute("speed").ToFloatEx();

            move.offsetTo = element.Attribute("offsetTo").ToVector3Ex();
        }
        else if (param.type == typeof(TimeEffectEntity))
        {
            var time = param as TimeEffectParam;
            time.duration= element.Attribute("duration").ToFloatEx();
            time.triggerAt = element.Attribute("triggerAt").ToFloatEx();
        }
        else if (param.type == typeof(HitEffectEntity))
        {
            var hit = param as HitEffectParam;
            hit.duration = element.Attribute("duration").ToFloatEx();
            hit.triggerAt = element.Attribute("triggerAt").ToFloatEx();
            hit.hitColor = element.Attribute("color").ToColorEx();
        }
    }
}

