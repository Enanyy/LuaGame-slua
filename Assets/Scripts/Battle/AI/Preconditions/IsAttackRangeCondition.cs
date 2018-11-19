using System;
using BTree;

public class IsAttackRangeCondition : BTPrecondition
{
    public IsAttackRangeCondition() { }
    public override bool Check(BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
            return input.CheckCondition(AIConditionType.IsAttackRange);
        }

        return false;

    }
}

