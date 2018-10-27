
using UnityEngine;
using BTree;
public class ActionMoveToPoint : BTAction
{
    public ActionMoveToPoint() : base() { }
    public ActionMoveToPoint(BTNode _parent) : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }

        PlayerController player = input.player;

        player.PlayAnimation("run",true);
        if(player.mNavMeshAgent.isStopped)
        {
            player.mNavMeshAgent.isStopped = false;
        }

        return base.OnExecute(ref _input);
    }
}
