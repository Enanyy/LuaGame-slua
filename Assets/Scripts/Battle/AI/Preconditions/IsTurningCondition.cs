using System;
using System.Collections.Generic;
using BTree;
public class IsTurningCondition : BTPrecondition
{
    public IsTurningCondition() { }

    public override bool Check(BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
            return input.CheckCondition(AIConditionType.IsTurning);
        }
        return false;
    }
}

