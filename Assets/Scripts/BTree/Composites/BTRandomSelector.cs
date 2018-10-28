
namespace BTree
{
    public class BTRandomSelector: BTPrioritySelector
    {
        public BTRandomSelector()
            : base()
        {
        }
        public BTRandomSelector(BTNode _parentNode, BTPrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {

        }

        protected override bool OnEvaluate(BTInput _input)
        {
            mCurrentSelectIndex = UnityEngine.Random.Range(0, childCount);
            if(CheckIndex(mCurrentSelectIndex))
            {
                var node = children[mCurrentSelectIndex];
                if (node.Evaluate(_input))
                {
                    return true;
                }
            }

            return base.OnEvaluate(_input);
        }
    }
}
