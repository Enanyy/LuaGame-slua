using System;
using System.Collections.Generic;

public class BattleData
{
    public List<HeroData> attackList = new List<HeroData>();
    public List<HeroData> defenseList = new List<HeroData>();

    public int AttackID;
    public int DefenseID;

    public bool IsAttacker = true;
    public float BattleTime = float.MaxValue;
   
}

