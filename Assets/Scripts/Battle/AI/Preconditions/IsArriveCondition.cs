using BTree;
using UnityEngine;
public class IsArriveCondition : BTPrecondition {

    public IsArriveCondition() { }

    public override bool Check(BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null )
        {
            return input.CheckCondition(AIConditionType.IsArrive);
        }
        return false;
    }

   
}
