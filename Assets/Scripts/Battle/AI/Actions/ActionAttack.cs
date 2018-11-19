using System;
using System.Collections.Generic;
using BTree;
public class ActionAttack : BTAction
{
    public ActionAttack():base() { }
    public ActionAttack(BTNode _parent)
            : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null )
        {
           return input.DoAction(AIActionType.Attack);
        }

        return base.OnExecute(ref _input);
    }
}

