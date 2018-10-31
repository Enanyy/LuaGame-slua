
using BTree;

namespace Battle.Logic
{
    public class StartActionNode : BTAction
    {
        public StartActionNode()
            : base()
        {

        }
        public StartActionNode(BTNode _parentNode)
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
           
            if (troop.count == 0)
            {
                troop.state = (int)TroopAnimState.Die;
                return BTResult.Executing;
            }
            if (troop.inPrepose)
            {
                if (troop.preTime > 0)
                {
                    troop.preTime = troop.preTime - 1;
                }
                else
                {
                    troop.inPrepose = false;
                }
                troop.state = (int)TroopAnimState.Idle;
                return BTResult.Executing;
            }
            if (troop.norAtkCD > 0)
            {
                troop.norAtkCD = troop.norAtkCD - 1;
                troop.state = (int)TroopAnimState.Idle;
                return BTResult.Executing;
            }
            troop.state = (int)TroopAnimState.Idle;
            return BTResult.Success;
        }
    }
}
