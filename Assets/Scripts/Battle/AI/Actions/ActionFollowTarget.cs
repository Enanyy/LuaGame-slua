using System;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class ActionFollowTarget : BTAction
{
    public ActionFollowTarget() : base() { }
    public ActionFollowTarget(BTNode _parent)
            : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
           return input.DoAction(AIActionType.FollowTarget);
        }
     
        return base.OnExecute(ref _input);
    }
}

