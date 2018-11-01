using System;
using System.Collections.Generic;
using BTree;
public class HasTargetCondition : BTPrecondition
{
    public HasTargetCondition() { }
    public override bool Check(BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {
            return false;
        }
        
        if(input.player.HasTarget())
        {
            return true;
        }

        return false;
    }
}

