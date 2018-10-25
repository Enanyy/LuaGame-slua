
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
           
        }

        public BTAction(BTNode _parentNode, BTPrecondition _precondition = null) 
            : base(_parentNode, _precondition)
        {
        }

        protected virtual void OnEnter(BTInput _input) { }
        protected virtual BTResult OnExecute(ref BTInput _input) { return BTResult.Success; }
        protected virtual void OnExit(BTInput _input, BTResult _status) { }


        protected override void OnTransition(BTInput _input)
        {
            if (mNeedExit)
            {
                OnExit(_input, BTResult.Failed);
            }
            mStatus = BTStatus.Ready;
            mNeedExit = false;
        }

        protected override BTResult OnTick(ref BTInput _input)
        {
            BTResult result = BTResult.Success;
            if (mStatus == BTStatus.Ready)
            {
                OnEnter(_input);
                mNeedExit = true;
                mStatus = BTStatus.Running;
            }
            if (mStatus == BTStatus.Running)
            {
                result = OnExecute(ref _input);
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
            }
            return result;
        }
    }
}
