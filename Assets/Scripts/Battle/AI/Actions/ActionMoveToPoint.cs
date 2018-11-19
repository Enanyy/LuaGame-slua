
using UnityEngine;
using BTree;
public class ActionMoveToPoint : BTAction
{
    public ActionMoveToPoint() : base() { }
    public ActionMoveToPoint(BTNode _parent)
            : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
           return  input.DoAction(AIActionType.MoveToPoint);
        }


        return base.OnExecute(ref _input);
    }
}
