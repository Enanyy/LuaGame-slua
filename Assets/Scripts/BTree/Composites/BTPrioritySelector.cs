
namespace BTree
{
    /// <summary>
    /// class:      带优先级的选择节点
    /// Evaluate:   从第一个子节点开始依次遍历所有的子节点，调用其Evaluate方法，当发现存在可以运行的子节点时，记录子节点索引，停止遍历，返回True。
    /// Tick:       调用可以运行的子节点的Tick方法，用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTPrioritySelector : BTComposite
    {
        protected int mCurrentSelectIndex = INVALID_CHILD_NODE_INDEX;
        protected int mLastSelectIndex = INVALID_CHILD_NODE_INDEX;
        public BTPrioritySelector()
            : base()
        {
        }
        public BTPrioritySelector(BTNode _parentNode, BTPrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {

        }

        protected override bool OnEvaluate(BTInput _input)
        {
            for (int i = 0; i < childCount; i++)
            {
                BTNode node = mChildren[i];
                if (node.Evaluate(_input))
                {
                    mCurrentSelectIndex = i;
                    return true;
                }
            }
            return false;
        }
        protected override void OnTransition(BTInput _input)
        {
            if (CheckIndex(mLastSelectIndex))
            {
                BTNode node = mChildren[mLastSelectIndex];
                node.Transition(_input);
            }
            mLastSelectIndex = INVALID_CHILD_NODE_INDEX;
        }
        protected override BTResult OnTick(ref BTInput _input)
        {
            BTResult result = BTResult.Success;
            if (CheckIndex(mCurrentSelectIndex))
            {
                if (mLastSelectIndex != mCurrentSelectIndex)
                {
                    if (CheckIndex(mLastSelectIndex))
                    {
                        BTNode node = mChildren[mLastSelectIndex];
                        node.Transition(_input);
                    }
                    mLastSelectIndex = mCurrentSelectIndex;
                }
            }
            if (CheckIndex(mLastSelectIndex))
            {
                BTNode node = mChildren[mLastSelectIndex];
                result = node.Tick(ref _input);
                if (result == BTResult.Success)
                {
                    mLastSelectIndex = INVALID_CHILD_NODE_INDEX;
                }
            }
            return result;
        }
    }
}
