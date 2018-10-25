
namespace BTree
{
    /// <summary>
    /// class:      序列节点
    /// Evaluate:   若是从头开始的，则调用第一个子节点的Evaluate方法，将其返回值作为自身的返回值返回。否则，调用当前运行节点的Evaluate方法，将其返回值作为自身的返回值返回。
    /// Tick:       调用可以运行的子节点的Tick方法，若返回运行结束，则将下一个子节点作为当前运行节点，若当前已是最后一个子节点，表示该序列已经运行结束，则自身返回运行结束。
    ///             若子节点返回运行中，则用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTSequence : BTComposite
    {
        private int mCurrentIndex = INVALID_CHILD_NODE_INDEX;
        public BTSequence()
            : base()
        {
        }
        public BTSequence(BTNode _parentNode, BTPrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {
        }

        protected override bool OnEvaluate(BTInput _input)
        {
            int i;
            if (mCurrentIndex == INVALID_CHILD_NODE_INDEX)
            {
                i = 0;
            }
            else
            {
                i = mCurrentIndex;
            }
            if (CheckIndex(i))
            {
                BTNode node = mChildren[i];
                if (node.Evaluate(_input))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnTransition(BTInput _input)
        {
            if (CheckIndex(mCurrentIndex))
            {
                BTNode node = mChildren[mCurrentIndex];
                node.Transition(_input);
            }
            mCurrentIndex = INVALID_CHILD_NODE_INDEX;
        }

        protected override BTResult OnTick(ref BTInput _input)
        {
            BTResult result = BTResult.Success;
            //First Time
            if (mCurrentIndex == INVALID_CHILD_NODE_INDEX)
            {
                mCurrentIndex = 0;
            }
            BTNode node = mChildren[mCurrentIndex];
            result = node.Tick(ref _input);
            if (result == BTResult.Success)
            {
                mCurrentIndex++;
                if (mCurrentIndex == childCount)
                {
                    mCurrentIndex = INVALID_CHILD_NODE_INDEX;
                }
                else
                {
                    result = BTResult.Executing;
                }
            }
            if (result == BTResult.Failed)
            {
                mCurrentIndex = INVALID_CHILD_NODE_INDEX;
            }
            return result;
        }
    }
}
