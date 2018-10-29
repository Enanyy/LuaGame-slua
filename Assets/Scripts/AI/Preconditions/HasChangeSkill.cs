using System;
using System.Collections.Generic;
using BTree;
public class HasChangeSkill : BTPrecondition
{
    public override bool Check(BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {
            return false;
        }
        return input.player.playerData.changeType != PlayerAnimationType.none;
    }
}

