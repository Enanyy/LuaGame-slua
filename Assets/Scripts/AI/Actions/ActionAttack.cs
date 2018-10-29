using System;
using System.Collections.Generic;
using BTree;
public class ActionAttack : BTAction
{
    public ActionAttack():base() { }
    public ActionAttack(BTNode _parent) : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }
        var player = input.player;
        if (player.playerData.changeType != PlayerAnimationType.none)
        {
            player.PlayAnimation(player.playerData.changeType, false);
            player.playerData.changeType = PlayerAnimationType.none;
            player.Stop();
        }
        return base.OnExecute(ref _input);
    }
}

