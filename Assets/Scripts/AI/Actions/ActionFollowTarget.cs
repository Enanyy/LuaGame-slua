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

        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }
        var player = input.player;
        if (player.IsDead() == false)
        {
            if (player.HasTarget())
            {
                var target = PlayerManager.GetSingleton().GetPlayer(player.data.target);
                if (target != null)
                {
                    player.SetDestination(target.transform.position);
                    player.data.stopDistance = player.data.attackDistance;
                }
            }
            else
            {
                player.SetDestination(player.transform.position);
               
            }
        }

        return base.OnExecute(ref _input);
    }
}

