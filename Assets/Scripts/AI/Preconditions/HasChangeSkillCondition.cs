using System;
using System.Collections.Generic;
using BTree;
public class HasChangeSkillCondition : BTPrecondition
{
    public HasChangeSkillCondition() { }
    public override bool Check(BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {
            return false;
        }
        return input.player.data.changeType != PlayerAnimationType.none;
    }
}

