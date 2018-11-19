using System;
using System.Collections.Generic;
using BTree;
public class IsDeadCondition : BTPrecondition
{
    public IsDeadCondition() { }

    public override bool Check(BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
            return input.CheckCondition(AIConditionType.IsDead);
        }
        return false;
    }
}

