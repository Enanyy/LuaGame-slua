using System;
using System.Collections.Generic;
using BTree;
public class ActionAttack : BTAction
{
    public ActionAttack():base() { }
    public ActionAttack(BTNode _parent) : base(_parent) { }

    protected override BTResult OnTick(ref BTInput _input)
    {
        return base.OnTick(ref _input);
    }
}

