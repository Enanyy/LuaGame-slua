
using BTree;

namespace Battle.Logic
{
    public class IdleActionNode : BTAction
    {
        public IdleActionNode()
            : base()
        {
        }
        public IdleActionNode(BTNode _parentNode) 
            : base(_parentNode)
        {
        }

        protected override void OnEnter(BTInput _input)
        {

        }

        protected override BTResult OnExecute(ref BTInput input)
        {
            BTInputData _input = input as BTInputData;
          
            if (_input == null)
            {
                Debugger.LogError("数据类型错误");
            }

            return BTResult.Executing;
        }
    }
}
