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
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }
        var player = input.player;
        if (player.data.changeType != PlayerAnimationType.none)
        {
            player.LookAtTarget();
            player.PlayAnimation(player.data.changeType,  UnityEngine.WrapMode.Default);
            player.data.changeType = PlayerAnimationType.none;
            player.Stop();
        }
        return base.OnExecute(ref _input);
    }
}

