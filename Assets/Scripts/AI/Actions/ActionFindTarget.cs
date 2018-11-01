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
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }
        var player = input.player;

        if(player.IsDead())
        {
            player.data.target = 0;

            return BTResult.Success;
        }

        if(player.FindTarget()==false)
        {
           
        }

        return base.OnExecute(ref _input);
    }
}

