
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
        if (player.navMeshAgent.destination != player.data.destination)
        {
            if (player.navMeshAgent.isOnNavMesh)
            {
                player.navMeshAgent.SetDestination(player.data.destination);
            }
        }
        player.PlayAnimation(PlayerAnimationType.run, true);
        player.Resume();

        return base.OnExecute(ref _input);
    }
}
