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
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }
        var player = input.player;

        player.PlayAnimation(PlayerAnimationType.death, UnityEngine.WrapMode.ClampForever);
        player.data.target = 0;
        player.Stop();
        player.navMeshAgent.enabled = false;

        return base.OnExecute(ref _input);
    }

}

