using BTree;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public static class PlayerConfig
{
    public static string path = Application.dataPath + "/R/Config/Player/";
    public static  byte[] ReadXML(string name)
    {
        string fullPath = string.Format("{0}{1}.xml", path , name );

        if (File.Exists(fullPath))
        {
            return File.ReadAllBytes(fullPath);
        }
        return null;
    }

    public static byte[] ReadBinary(string name)
    {
        string fullPath = string.Format("{0}{1}.btree", path, name);

        if (File.Exists(fullPath))
        {
            return File.ReadAllBytes(fullPath);
        }
        return null;
    }

    public static void Init()
    {
        BTFactory.RegisterActionType(typeof(ActionIdle));
        BTFactory.RegisterActionType(typeof(ActionMoveToPoint));
        BTFactory.RegisterActionType(typeof(ActionAttack));
        BTFactory.RegisterActionType(typeof(ActionFindTarget));
        BTFactory.RegisterActionType(typeof(ActionFollowTarget));
        BTFactory.RegisterActionType(typeof(ActionRandomSkill));
        BTFactory.RegisterActionType(typeof(ActionDie));

        BTFactory.RegisterPreconditionType(typeof(IsArrivedCondition));
        BTFactory.RegisterPreconditionType(typeof(IsReleaseSkillCondition));
        BTFactory.RegisterPreconditionType(typeof(HasChangeSkillCondition));
        BTFactory.RegisterPreconditionType(typeof(HasTargetCondition));
        BTFactory.RegisterPreconditionType(typeof(IsDeadCondition));
    }
}

