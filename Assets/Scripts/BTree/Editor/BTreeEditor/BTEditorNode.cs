
using UnityEngine;

namespace BTree.Editor
{
    public class BTEditorNode
    {
        public BTNode mNode;
        public Vector2 mPos;
        public bool mDisable;
        public bool mIsCollapsed;//损坏
        public BTEditorNode(BTNode _node)
        {
            mNode = _node;
        }
        public void AddChildNode(BTEditorNode _node)
        {
            mNode.AddChild(_node.mNode);
        }
        public void RemoveChildNode(BTEditorNode _node)
        {
            mNode.RemoveChild(_node.mNode);
        }
    }
}
