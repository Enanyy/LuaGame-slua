using BTree;
using UnityEngine;
public class IsArrivedCondition : BTPrecondition {

    public IsArrivedCondition() { }

    public override bool Check(BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input==null || input.player == null)
        {
            return false;
        }

        PlayerEntity player = input.player;
       
        if (player.IsArrivedDestination())
        {
            player.data.destination = player.transform.position;
            return true;
        }

        return false;
    }

   
}
