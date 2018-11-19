using System;
using System.Collections.Generic;

public class HeroActionDiePlugin : HeroActionPlugin
{
    public override void OnExit()
    {
        base.OnExit();
        HeroEntity hero = action.entity;

        BattleManager.instance.RemoveEntity(action.entity.data.id);
    }
}

