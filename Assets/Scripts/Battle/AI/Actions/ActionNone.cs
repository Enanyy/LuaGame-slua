using System;
using System.Collections.Generic;
using BTree;
public class ActionNone : BTAction
{
    public ActionNone() : base() { }
    public ActionNone(BTNode _parent)
            : base(_parent) { }
    protected override BTResult OnExecute(ref BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
            return input.DoAction(AIActionType.None);
        }

        return base.OnExecute(ref _input);
    }
}

