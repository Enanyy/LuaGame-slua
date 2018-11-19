using System;
using System.Collections.Generic;
using System.IO;
using BTree;
using UnityEngine;

public static class AIConfig
{
   
    public static void Init()
    {
        BTFactory.RegisterActionType(typeof(ActionIdle));
        BTFactory.RegisterActionType(typeof(ActionMoveToPoint));
        BTFactory.RegisterActionType(typeof(ActionAttack));
        BTFactory.RegisterActionType(typeof(ActionFindTarget));
        BTFactory.RegisterActionType(typeof(ActionFollowTarget));
        BTFactory.RegisterActionType(typeof(ActionDie));

        BTFactory.RegisterPreconditionType(typeof(IsArriveCondition));
        BTFactory.RegisterPreconditionType(typeof(IsAttackingCondition));
        BTFactory.RegisterPreconditionType(typeof(IsAttackRangeCondition));
        BTFactory.RegisterPreconditionType(typeof(HasAttackCondition));
        BTFactory.RegisterPreconditionType(typeof(HasTargetCondition));
        BTFactory.RegisterPreconditionType(typeof(IsDeadCondition));
        BTFactory.RegisterPreconditionType(typeof(IsTurningCondition));
        BTFactory.RegisterPreconditionType(typeof(IsMovingCondition));
    }
   

    public static  void ReadXML(string name,BTRoot root)
    {
        string assetBundleName = string.Format("assets/assetbundle/config/character/ai/{0}.xml", name.ToLower());
        AssetCache.LoadAssetAsync<TextAsset>(assetBundleName, assetBundleName, (asset) =>
        {
            if (asset != null)
            {
              
                TextAsset text = asset.GetAsset() as TextAsset;

                if (text!=null)
                {
                    root.InitXML(text.bytes);
                }
            }
        });
            
    }

    public static void  ReadBinary(string name,BTRoot root)
    {
        string assetBundleName = string.Format("assets/assetbundle/config/character/ai/{0}.bytes", name.ToLower());

        AssetCache.LoadAssetAsync<TextAsset>(assetBundleName, assetBundleName, (asset) =>
        {
            if (asset != null)
            {

                TextAsset text = asset.GetAsset() as TextAsset;

                if (text != null)
                {
                    root.InitBinary(text.bytes);
                }
            }
        });
    }
}

