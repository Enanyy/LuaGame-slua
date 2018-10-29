
namespace BTree
{
    public class BTRandomSelector: BTPrioritySelector
    {
        private System.Random mRandom = new System.Random();
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
            mCurrentSelectIndex = mRandom.Next(0, childCount);
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

        public void SetSeed(int seed)
        {
            mRandom = new System.Random(seed);
        }
    }
}
