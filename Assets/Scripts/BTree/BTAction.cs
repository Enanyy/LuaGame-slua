
//执行节点基类
namespace BTree
{
    public abstract class BTAction : BTNode
    {
        private BTStatus mStatus = BTStatus.Ready;
        private bool mNeedExit = false;

        public BTAction()
            :base()
        {
            isAcitonNode = true;
        }

        public BTAction(BTNode _parentNode, BTPrecondition _precondition = null) 
            : base(_parentNode, _precondition)
        {
            isAcitonNode = true;
        }

        protected virtual void OnEnter(BTData _input) { }
        protected virtual BTResult OnExecute(ref BTData _input) { return BTResult.Success; }
        protected virtual void OnExit(BTData _input, BTResult _status) { }


        protected override void OnTransition(BTData _input)
        {
            if (mNeedExit)
            {
                OnExit(_input, BTResult.Failed);
            }
            SetActiveNode(null);
            mStatus = BTStatus.Ready;
            mNeedExit = false;
        }

        protected override BTResult OnTick(ref BTData _input)
        {
            BTResult result = BTResult.Success;
            if (mStatus == BTStatus.Ready)
            {
                OnEnter(_input);
                mNeedExit = true;
                mStatus = BTStatus.Running;
                SetActiveNode(this);
            }
            if (mStatus == BTStatus.Running)
            {
                result = OnExecute(ref _input);
                SetActiveNode(this);
                if (result == BTResult.Success || result == BTResult.Failed)
                {
                    mStatus = BTStatus.Finish;
                }
            }
            if (mStatus == BTStatus.Finish)
            {
                if (mNeedExit)
                {
                    OnExit(_input, result);
                }
                mStatus = BTStatus.Ready;
                mNeedExit = false;
                SetActiveNode(null);
            }
            return result;
        }
    }
}
