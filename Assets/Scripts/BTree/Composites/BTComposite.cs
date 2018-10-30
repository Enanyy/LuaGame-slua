
namespace BTree
{
    /// <summary>
    /// 混合节点
    /// </summary>
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

        public override int GetWeight()
        {
            int weight = -1;

            for (int i = 0; i < childCount; i++)
            {
                BTNode node = mChildren[i];

                int w = node.GetWeight();
                if (weight == -1 || w > weight)
                {
                    weight = w;
                }

            }
            return weight;
        }
    }
}
