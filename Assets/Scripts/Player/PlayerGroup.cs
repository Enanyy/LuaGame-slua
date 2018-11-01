using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerGroup : EntityBase<PlayerGroupData>
{
    public List<PlayerEntity> playerList { get; private set; }


    public override void SetData(PlayerGroupData _data)
    {
        base.SetData(_data);
        Init();
       
    }

    private void Init()
    {
        playerList = new List<PlayerEntity>();

        for(int i = 0; i < data.count; ++i)
        {
           
            PlayerData playerData = new PlayerData();
            playerData.id = data.id *100 +i;
            playerData.camp = data.camp;

            playerData.config = "AI_Akali";
            playerData.model = data.model;
            playerData.hp = data.hp;
          
            int column = i / data.columns;
            int row = i % data.columns;

            float offset = 0;

            if (data.columns % 2 == 1)
            {
                offset = row - data.columns / 2;
            }
            else
            {
                offset = row - data.columns / 2 + 0.5f;
            }

            playerData.x = data.x + offset * data.spaceRow;
            playerData.z = data.z + column * data.spaceColumn;
            playerData.dirX = data.dirX;
            playerData.dirZ = data.dirZ;

            var player = PlayerManager.GetSingleton().CreatePlayer(playerData);

            playerData.destination = player.transform.position;

            playerList.Add(player);
 
        }
    }

    public void Tick(float deltaTime)
    {

    }
   
}

