using System;
using System.Collections.Generic;
using UnityEngine;
using BTree;
public class ActionFindTarget : BTAction
{
    public ActionFindTarget() : base() { }
    public ActionFindTarget(BTNode _parent) : base(_parent) { }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        PlayerInputData input = _input as PlayerInputData;
        if (input == null || input.player == null)
        {

        }
        var player = input.player;
        var players = PlayerManager.GetSingleton().players;
        float minDistance = float.MaxValue;
        int minTarget = -1;
        for(int i = 0;  i < players.Count; ++i)
        {
            if (players[i].data.camp != player.data.camp)
            {
                if(minTarget == -1)
                {
                    minTarget = players[i].data.id;
                    minDistance = Vector3.Distance(players[i].transform.position, player.transform.position);
                }
                else
                {
                    float distance = Vector3.Distance(players[i].transform.position, player.transform.position);
                    if(distance <minDistance)
                    {
                        minTarget= players[i].data.id;
                        minDistance = distance;
                    }
                }
            }
        }
        if(minTarget !=-1)
        {
            player.data.target = minTarget;
        }

        return base.OnExecute(ref _input);
    }
}

