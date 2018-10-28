using System;

namespace BTree
{
    public class BTDecorator : BTComposite
    {
        public BTDecorator()
            : base()
        {
            maxChild = 1;
        }
        public BTDecorator(BTNode _parentNode, BTPrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {
            maxChild = 1;
        }
    }
}
