
namespace BTree
{
    //选择节点基类
    public class BTComposite : BTNode
    {
        public BTComposite()
            : base()
        {

        }
        public BTComposite(BTNode _parentNode, BTPrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {

        }
    }
}
