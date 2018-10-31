

using BTree;
using System.Collections.Generic;

namespace Battle.Logic
{
    public class FindTargetActionNode : BTAction
    {
        public FindTargetActionNode()
            : base()
        {

        }
        public FindTargetActionNode(BTNode _parentNode) 
            : base(_parentNode)
        {
        }
        
        protected override BTResult OnExecute(ref BTInput input)
        {
            BTInputData _input = input as BTInputData;
         
            if (_input == null)
            {
                Debugger.LogError("数据类型错误");
            }

            var troop = _input.troop;

            List<TroopData> enemys;
            if (troop.isAtkTroop)
            {
                enemys = _input.battleData.mDefTroopList;
            }
            else
            {
                enemys = _input.battleData.mAtcTroopList;
            }
            //找最近的目标
            int minDis = -1;
            uint targetKey = 0;
            for (int i = 0; i < enemys.Count; i++)
            {
                int dis = MathHelper.TroopDistanceV2(troop, enemys[i]);
                if (minDis < 0)
                {
                    minDis = dis;
                    targetKey = enemys[i].key;
                }
                else if (minDis > dis)
                {
                    minDis = dis;
                    targetKey = enemys[i].key;
                }
            }
            troop.targetKey = targetKey;
            return BTResult.Success;
        }
    }
}
