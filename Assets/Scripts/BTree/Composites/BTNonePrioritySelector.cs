
namespace BTree
{
    /// <summary>
    /// class:      不带优先级的选择节点
    /// Evaluate:   先调用上一个运行的子节点（若存在）的Evaluate方法，如果可以运行，则继续运保存该节点的索引，返回True，如果不能运行，则重新选择（同带优先级的选择节点的选择方式）
    /// Tick:       调用可以运行的子节点的Tick方法，用它所返回的运行状态作为自身的运行状态返回
    /// </summary>
    public class BTNonePrioritySelector : BTPrioritySelector
    {
        public BTNonePrioritySelector()
            : base()
        {
        }
        public BTNonePrioritySelector(BTNode _parentNode, BTPrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {
        }
        protected override bool OnEvaluate(BTData _input)
        {
            if (CheckIndex(mCurrentSelectIndex))
            {
                BTNode node = mChildren[mCurrentSelectIndex];
                if (node.Evaluate(_input))
                {
                    return true;
                }
            }
            return base.OnEvaluate(_input);
        }
    }
}
