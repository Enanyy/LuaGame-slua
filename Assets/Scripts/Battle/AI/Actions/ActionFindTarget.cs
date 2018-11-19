using System;
using System.Collections.Generic;
using UnityEngine;
using BTree;
public class ActionFindTarget : BTAction
{
    public ActionFindTarget() : base() { }
    public ActionFindTarget(BTNode _parent)
            : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null )
        {
          return  input.DoAction(AIActionType.FindTarget);
        }


        return base.OnExecute(ref _input);
    }
}

