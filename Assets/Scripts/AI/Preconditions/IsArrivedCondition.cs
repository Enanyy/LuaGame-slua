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

        PlayerController player = input.player;
        Vector3 position = player.transform.position;
        position.y = player.data.destination.y;
        float distance = Vector3.Distance(player.data.destination, position);
        if (distance <= player.data.stopDistance)
        {
            player.data.destination = player.transform.position;
            return true;
        }

        return false;
    }

   
}
