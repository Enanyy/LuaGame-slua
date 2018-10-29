using System;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class ActionFollowTarget : BTAction
{
    public ActionFollowTarget() : base() { }
    public ActionFollowTarget(BTNode _parent) : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {

        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }
        var player = input.player;

        if(player.data.target >=0)
        {
            var target = PlayerManager.GetSingleton().GetPlayer(player.data.target);
            if(target!=null)
            {
                Vector3 direction = (target.transform.position - player.transform.position).normalized;
                Vector3 targetPosition = target.transform.position - direction * 2;

                player.MoveToPoint(targetPosition);
            }
        }

        return base.OnExecute(ref _input);
    }
}

