
namespace BTree
{
    public class BTRandomSelector: BTPrioritySelector
    {
        public int seed { get; private set; }
        private System.Random mRandom;
        public const int DEFAULT_SEED = 0;
        public BTRandomSelector()
            : base()
        {
            seed = DEFAULT_SEED;
            mRandom = new System.Random(seed);
            
        }
        public BTRandomSelector(BTNode _parentNode, BTPrecondition _precondition = null,int _seed = DEFAULT_SEED)
            : base(_parentNode, _precondition)
        {
            seed = _seed;
            mRandom = new System.Random(seed);
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
    }
}
