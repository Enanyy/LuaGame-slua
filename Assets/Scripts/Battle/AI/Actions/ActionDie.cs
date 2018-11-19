using System;
using System.Collections.Generic;
using BTree;


public class ActionDie : BTAction
{
    public ActionDie():base() { }
    public ActionDie(BTNode _parent)
            : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
            return input.DoAction(AIActionType.Die);
        }


        return base.OnExecute(ref _input);
    }

}

