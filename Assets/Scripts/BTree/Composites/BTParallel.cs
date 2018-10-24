
using System.Collections.Generic;
namespace BTree
{
    /// <summary>
    /// class:      并行节点
    /// Evaluate:   依次调用所有的子节点的Evaluate方法，若所有的子节点都返回True，则自身也返回True，否则，返回False
    /// Tick:       调用所有子节点的Tick方法，若并行节点是“或者”的关系，则只要有一个子节点返回运行结束，那自身就返回运行结束。
    ///             若并行节点是“并且”的关系，则只有所有的子节点返回结束，自身才返回运行结束
    /// </summary>
    public class BTParallel : BTComposite
    {
        public BTParallelCondition mFinishCondition;
        private List<BTResult> mChildrenResults = new List<BTResult>();
        public BTParallel()
            : base()
        {
        }
        public BTParallel(BTNode _parentNode, BTPrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {
        }

        protected override bool OnEvaluate(BTData _input)
        {
            for (int i = 0; i < childCount; i++)
            {
                BTNode node = mChildren[i];
                if (mChildrenResults[i] == BTResult.Executing)
                {
                    if (!node.Evaluate(_input))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected override void OnTransition(BTData _input)
        {
            for (int i = 0; i < childCount; i++)
            {
                mChildrenResults[i] = BTResult.Executing;
                BTNode node = mChildren[i];
                node.Transition(_input);
            }
        }

        protected override BTResult OnTick(BTData _input, ref BTData _output)
        {
            int finishedChildCount = 0;
            for (int i = 0; i < childCount; i++)
            {
                BTNode node = mChildren[i];
                if (mFinishCondition == BTParallelCondition.OR)
                {
                    if (mChildrenResults[i] == BTResult.Executing)
                    {
                        mChildrenResults[i] = node.Tick(_input, ref _output);
                    }
                    if (mChildrenResults[i] != BTResult.Executing)
                    {
                        Reset();
                        return BTResult.Success;
                    }
                }
                else if (mFinishCondition == BTParallelCondition.AND)
                {
                    if (mChildrenResults[i] == BTResult.Executing)
                    {
                        mChildrenResults[i] = node.Tick(_input, ref _output);
                    }
                    if (mChildrenResults[i] != BTResult.Executing)
                    {
                        finishedChildCount++;
                    }
                }
                else
                {
                    Debugger.LogError("BTreeParallelFinishCondition Error");
                }
            }
            if (finishedChildCount == childCount)
            {
                Reset();
                return BTResult.Success;
            }
            return BTResult.Executing;
        }

        public override void AddChild(BTNode _childNode)
        {
            base.AddChild(_childNode);
            mChildrenResults.Add(BTResult.Executing);
        }

        protected void Reset()
        {
            for (int i = 0; i < childCount; i++)
            {
                mChildrenResults[i] = BTResult.Executing;
            }
        }
    }
}
