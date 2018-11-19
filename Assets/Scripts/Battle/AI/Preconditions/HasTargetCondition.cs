using System;
using System.Collections.Generic;
using BTree;
public class HasTargetCondition : BTPrecondition
{
    public HasTargetCondition() { }
    public override bool Check(BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null )
        {
            return input.CheckCondition(AIConditionType.HasTarget);
        }
        return false;
    }
}

