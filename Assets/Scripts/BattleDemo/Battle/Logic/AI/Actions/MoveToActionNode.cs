
using BTree;

namespace Battle.Logic
{
    public class MoveToActionNode : BTAction
    {
        public MoveToActionNode()
            : base()
        {
        }
        public MoveToActionNode(BTNode _parentNode)
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

            if (_input.troop.targetKey!=0)
            {
                _input.troop.state = (int)TroopAnimState.Move;
                MoveToTarget(ref _input);
            }
            return BTResult.Success;
        }

        private void MoveToTarget(ref BTInputData _input)
        {
            var troop = _input.troop;
          
            var target = _input.battleData.mAllTroopDic[troop.targetKey];
            var x = troop.x;
            var y = troop.y;
            var tar_x = target.x;
            var tar_y = target.y;

            troop.dir_x = tar_x;
            troop.dir_y = tar_y;

            if (x > tar_x)
            {
                troop.x = x - 1;
                if (troop.x < tar_x)
                {
                    troop.x = tar_x;
                }
            }
            else if (x < tar_x)
            {
                troop.x = x + 1;
                if (troop.x > tar_x)
                {
                    troop.x = tar_x;
                }
            }
            if (y > tar_y)
            {
                troop.y = y - 1;
                if (troop.y < tar_y)
                {
                    troop.y = tar_y;
                }
            }
            else if (y < tar_y)
            {
                troop.y = y + 1;
                if (troop.y > tar_y)
                {
                    troop.y = tar_y;
                }
            }
        }
    }

}
