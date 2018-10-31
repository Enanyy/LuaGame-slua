
using BTree;
using Battle.Logic;
namespace Battle.Logic
{
    public class BTInputData : BTInput
    {
        public TroopData troop;
        public BattleData battleData;
        public BTInputData()
        {
        }
        public void SetData(TroopData _troop, BattleData _battleData)
        {
            troop = _troop;
            battleData = _battleData;
        }
    }
   
}
