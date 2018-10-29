using System;
using System.Collections.Generic;


namespace BTree
{
    public  class BTWeigthSelector : BTPrioritySelector
    {
        public BTWeigthSelector() : base() { }
        public BTWeigthSelector(BTNode _parent) : base(_parent) { }

        protected override bool OnEvaluate(BTInput _input)
        {
            mCurrentSelectIndex = INVALID_CHILD_NODE_INDEX;

            int weight = -1;

            for (int i = 0; i < childCount; i++)
            {
                BTNode node = mChildren[i];
                if (node.Evaluate(_input))
                {
                    if (weight == -1)
                    {
                        weight = node.weight;
                        mCurrentSelectIndex = i;
                    }
                    else
                    {
                        if (node.weight > weight)
                        {
                            weight = node.weight;
                            mCurrentSelectIndex = i;
                        }
                    }
                }
            }
            if (CheckIndex(mCurrentSelectIndex))
            {
                return true;
            }

            return base.OnEvaluate(_input);
        }
    }
}
