using System;
using System.Collections.Generic;
using BTree;
public class ActionRandomSkill : BTAction
{
    private Random mRandom = new Random();

    public ActionRandomSkill() : base() { }
    public ActionRandomSkill(BTNode _parent) : base(_parent) { }
    protected override BTResult OnExecute(ref BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }

        var player = input.player;
        if(player.IsReleaseSkill()==false && player.HasChangeSkill()==false && player.IsArrivedDestination())
        {
            int value = mRandom.Next(0, 4);
            switch(value)
            {
                case 0:player.data.changeType = PlayerAnimationType.attack1;break;
                case 1:player.data.changeType = PlayerAnimationType.attack2;break;
                case 2:player.data.changeType = PlayerAnimationType.spell1;break;
                case 3:player.data.changeType = PlayerAnimationType.spell3;break;
               
            } 
        }

        return base.OnExecute(ref _input);
    }
}

