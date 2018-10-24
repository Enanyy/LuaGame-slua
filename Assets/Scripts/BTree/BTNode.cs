
using System.Collections.Generic;

namespace BTree
{
    public abstract class BTNode
    {   
        //节点名称
        public string name = "UNNAMED";
        //节点索引
        public int index { get; set; }
        //前提条件
        public BTPrecondition precondition { get; private set; }

        //子节点
        protected List<BTNode> mChildren = new List<BTNode>();
        public List<BTNode> children
        {
            get
            {
                List<BTNode> _children = new List<BTNode>(mChildren);
                return _children;
            }
        }
        //子节点数
        public int childCount{ get { return mChildren.Count; }  }
        //父节点
        public BTNode parent { get; protected set; }
        //上一个激活的节点
        public BTNode lastActiveNode { get; private set; }
        //当前激活的节点
        public BTNode activeNode { get; private set; }
        //是否是Action节点
        public bool isAcitonNode { get; protected set; }

        protected const int MAX_CHILD_NODE_COUNT = 16;
        protected const int INVALID_CHILD_NODE_INDEX = -1;

        //必须有个无参的构造函数
        public BTNode()
        {
            name = GetType().Name;
            parent = null;
            precondition = null;
            isAcitonNode = false;
        }
        public BTNode(BTNode _parentNode, BTPrecondition _precondition = null)
        {
            parent = _parentNode;
            precondition = _precondition;
            isAcitonNode = false;
        }

        public bool Evaluate(BTData _input)
        {
            return (precondition == null 
                || precondition.Check(_input)) 
                && OnEvaluate(_input);
        }

        public void Transition(BTData _input)
        {
            OnTransition(_input);
        }

        public BTResult Tick(BTData _input, ref BTData _output)
        {
            return OnTick(_input, ref _output);
        }

        public virtual void AddChild(BTNode _childNode)
        {
            if (childCount>= MAX_CHILD_NODE_COUNT)
            {
                Debugger.LogError("添加行为树节点失败：超过最大数量16");
                return;
            }
            mChildren.Add(_childNode);
        }

        public virtual void RemoveChild(BTNode _childNode)
        {
            for (int i = 0; i < childCount; i++)
            {
                if (mChildren[i].Equals(_childNode))
                {
                    mChildren.RemoveAt(i);
                    break;
                }
            }
        }

        public BTNode SetPrecondition(BTPrecondition _NodePrecondition)
        {
            if (precondition != _NodePrecondition)
            {
                precondition = _NodePrecondition;
            }
            return this;
        }
       
        public void SetActiveNode(BTNode _node)
        {
            lastActiveNode = activeNode;
            activeNode = _node;
            if (parent != null)
                parent.SetActiveNode(_node);
        }

        protected virtual bool OnEvaluate(BTData _input)
        {
            return true;
        }
        protected virtual void OnTransition(BTData _input)
        {
        }
        protected virtual BTResult OnTick(BTData _input, ref BTData _output)
        {
            return BTResult.Success;
        }

        protected bool CheckIndex(int _index)
		{
			return _index >= 0 && _index < childCount;
		}
}
}
