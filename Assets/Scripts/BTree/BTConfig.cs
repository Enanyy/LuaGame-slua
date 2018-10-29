
using System;

namespace BTree
{
    [Serializable]
    public class TreeConfig
    {
        public string mName;
        public TreeNodeConfig[] mNodes;
    }
    [Serializable]
    public class TreeNodeConfig
    {
        public int mParentIndex = -1;
        public int mNodeType;
        public int mNodeSubType;
        public int[] mOtherParams;
        public string mActionNodeName;
        public string mNodeName;
        public PreconditionConfig[] mPreconditions;
    }
    [Serializable]
    public class PreconditionConfig
    {
        public int mParentIndex = -1;
        public int mType;
        public string mPreconditionName;
        public int[] mChildIndexs;
    }

    public enum NodeType
    {
        SelectorNode = 1,
        ActionNode = 2,
    }
    public enum SelectorNodeType
    {
        BTParallel = 1,
        BTPrioritySelector = 2,
        BTNonePrioritySelector = 3,
        BTSequence = 4,
        BTLoop = 5,
        BTRandomSelector = 6,
        BTWeigthSelector = 7,
        BTDecorator = 8,
    }
    public enum PreconditionType
    {
        And = 1,
        Or = 2,
        Not = 3,
    }
}
