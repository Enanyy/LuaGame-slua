
using Battle.Logic;
using BTree;

namespace Battle.Logic
{
    public class AttackActionNode : BTAction
    {
        public AttackActionNode()
            : base()
        {
        }
        public AttackActionNode(BTNode _parentNode) 
            : base(_parentNode)
        {
        }

        protected override BTResult OnExecute(ref BTInput input)
        {
            BTInputData _input = input as BTInputData;
            
            if (_input == null )
            {
                Debugger.LogError("数据类型错误");
            }

            var troop = _input.troop;
            var target = _input.battleData.mAllTroopDic[troop.targetKey];
            var tar_x = target.x;
            var tar_y = target.y;

            troop.dir_x = tar_x;
            troop.dir_y = tar_y;

            troop.state = (int)TroopAnimState.Attack;
            troop.inPrepose = true;
            troop.preTime = TroopHelper.GetTroopAtkPrepareTime(troop.type);
            troop.isAtk = true;
            troop.norAtkCD = TroopHelper.GetTroopAtkCDTime(troop.type);
            return BTResult.Success;
        }
    }
}
