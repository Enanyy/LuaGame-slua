using System;
using System.Collections.Generic;
using BTree;
public class HasAttackCondition : BTPrecondition
{
    public HasAttackCondition() { }
    public override bool Check(BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null )
        {
            return input.CheckCondition(AIConditionType.HasAttack);
        }

        return false;
    }
}

