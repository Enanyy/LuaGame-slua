using System;
using BTree;

public class IsMovingCondition : BTPrecondition
{
    public IsMovingCondition() { }

    public override bool Check(BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
            return input.CheckCondition(AIConditionType.IsMoving);
        }
        return false;
    }
}

