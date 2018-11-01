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
          
            playerData.animationsLength = new Dictionary<PlayerAnimationType, float> {
            { PlayerAnimationType.attack1,1.250f },
            { PlayerAnimationType.attack2,1.250f },
            { PlayerAnimationType.dance,8.875f },
            { PlayerAnimationType.die,1.750f },
            { PlayerAnimationType.idle,1.250f },
            { PlayerAnimationType.run,0.833f },
            { PlayerAnimationType.sneak,0.583f },
            { PlayerAnimationType.spell1,1.250f },
            { PlayerAnimationType.spell3,1.250f } };



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

