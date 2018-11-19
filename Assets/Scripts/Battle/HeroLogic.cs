using System;
using System.Collections.Generic;
using BTree;
public class HeroLogic
{
    Dictionary<string, BTRoot> mBattleLogicDic = new Dictionary<string, BTRoot>();

    public void Tick(string config, ref BTInput input )
    {
        if(string.IsNullOrEmpty(config))
        {
            return;
        }
        if(mBattleLogicDic.ContainsKey(config)==false)
        {
            BTRoot root = new BTRoot();
            AIConfig.ReadXML(config, root);
            mBattleLogicDic.Add(config, root);
        }
        mBattleLogicDic[config].Tick(ref input);
    }

    public void Destroy()
    {
        mBattleLogicDic.Clear();
    }
}

