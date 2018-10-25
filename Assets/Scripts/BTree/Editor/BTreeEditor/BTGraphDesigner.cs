
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace BTree.Editor
{
    public class BTGraphDesigner
    {
        //public BTreeNodeDesigner m_EntryNode { get; private set; }
        public BTNodeDesigner mRootNode { get; private set; }
        public List<BTNodeDesigner> mDetachedNodes = new List<BTNodeDesigner>();
        public List<BTNodeDesigner> mSelectedNodes = new List<BTNodeDesigner>();
        public BTNodeDesigner mHoverNode { get; private set; }
        public BTNodeConnection mActiveNodeConnection { get; set; }
        public List<BTNodeConnection> mSelectedNodeConnections = new List<BTNodeConnection>();

        #region 绘制相关
        public bool DrawNodes(Vector2 mousePosition, Vector2 offset, float graphZoom)
        {
            //if (m_EntryNode == null)
            //{
            //    return false;
            //}
            //m_EntryNode.drawNodeConnection(offset, graphZoom, false);
            //从根节点开始递归绘制
            
            if (mRootNode != null)
            {
                DrawNodeConnectionChildren(mRootNode, offset, graphZoom, mRootNode.mEditorNode.mDisable);
            }
            //绘制未连接的节点
            for (int i = 0; i < mDetachedNodes.Count; i++)
            {
                DrawNodeConnectionChildren(mDetachedNodes[i], offset, graphZoom, mDetachedNodes[i].mEditorNode.mDisable);
            }
            //绘制选中的连线
            for (int i = 0; i < mSelectedNodeConnections.Count; i++)
            {
                mSelectedNodeConnections[i].DrawConnection(offset, graphZoom, mSelectedNodeConnections[i].mOriginatingNodeDesigner.mEditorNode.mDisable);
            }
            //
            if (mousePosition != new Vector2(-1f, -1f) && mActiveNodeConnection != null)
            {
                mActiveNodeConnection.HorizontalHeight = (mActiveNodeConnection.mOriginatingNodeDesigner.GetConnectionPosition(offset, mActiveNodeConnection.NodeConnectionType).y + mousePosition.y) / 2;
                var _offset = mActiveNodeConnection.mOriginatingNodeDesigner.GetConnectionPosition(offset, mActiveNodeConnection.NodeConnectionType);
                var _disable = mActiveNodeConnection.mOriginatingNodeDesigner.mEditorNode.mDisable && mActiveNodeConnection.NodeConnectionType == NodeConnectionType.Outgoing;
                mActiveNodeConnection.DrawConnection(_offset, mousePosition, graphZoom, _disable);
            }
            
            //m_EntryNode.drawNode(offset, false, false);
            bool result = false;
            //绘制跟节点
            if (mRootNode != null && DrawNodeChildren(mRootNode, offset, mRootNode.mEditorNode.mDisable))
            {
                result = true;
            }
            //绘制未连接的节点
            for (int i = 0; i < mDetachedNodes.Count; i++)
            {
                if (DrawNodeChildren(mDetachedNodes[i], offset, mDetachedNodes[i].mEditorNode.mDisable))
                {
                    result = true;
                }
            }
            //绘制选中的节点
            for (int i = 0; i < mSelectedNodes.Count; i++)
            {
                if (DrawNodeChildren(mSelectedNodes[i], offset, mSelectedNodes[i].mEditorNode.mDisable))
                {
                    result = true;
                }
            }
            //绘制根节点的说明
            if (mRootNode != null)
            {
                DrawNodeCommentChildren(mRootNode, offset);
            }
            //绘制分离节点说明
            for (int i = 0; i < mDetachedNodes.Count; i++)
            {
                DrawNodeCommentChildren(mDetachedNodes[i], offset);
            }
            return result;
        }
        //绘制临时连线
        public void DrawTempConnection(Vector2 destination, Vector2 offset, float graphZoom)
        {
            if (mSelectedNodes != null && mSelectedNodes.Count == 1)
            {
                Color color = Color.red;
                Handles.color = color;
                Vector3[] array = new Vector3[]
                {
                    mSelectedNodes[0].GetConnectionPosition(offset,NodeConnectionType.Outgoing),
                    destination
                };
                Handles.DrawAAPolyLine(BTEditorUtility.TaskConnectionTexture, 1f / graphZoom, array);
            }
        }
        //递归绘制连线
        private void DrawNodeConnectionChildren(BTNodeDesigner nodeDesigner, Vector2 offset, float graphZoom, bool disabledNode)
        {
            if (nodeDesigner == null)
            {
                return;
            }
            if (!nodeDesigner.mEditorNode.mIsCollapsed)
            {
                nodeDesigner.DrawNodeConnection(offset, graphZoom, nodeDesigner.mEditorNode.mDisable || disabledNode);
                if (nodeDesigner.mChildNodeList != null)
                {
                    for (int i = 0; i < nodeDesigner.mChildNodeList.Count; i++)
                    {
                        var _child = nodeDesigner.mChildNodeList[i];
                        DrawNodeConnectionChildren(_child, offset, graphZoom, _child.mEditorNode.mDisable || disabledNode);
                    }
                }
            }
        }
        //递归绘制节点说明
        private void DrawNodeCommentChildren(BTNodeDesigner nodeDesigner, Vector2 offset)
        {
            if (nodeDesigner == null)
            {
                return;
            }
            nodeDesigner.DrawNodeComment(offset);
            if (nodeDesigner.mChildNodeList != null)
            {
                for (int i = 0; i < nodeDesigner.mChildNodeList.Count; i++)
                {
                    var _child = nodeDesigner.mChildNodeList[i];
                    DrawNodeCommentChildren(_child, offset);
                }
            }
        }
        //递归绘制节点
        private bool DrawNodeChildren(BTNodeDesigner nodeDesigner, Vector2 offset, bool disabledNode)
        {
            if (nodeDesigner == null)
            {
                return false;
            }
            bool result = false;
            if (nodeDesigner.DrawNode(offset, false, disabledNode))
            {
                result = true;
            }
            if (nodeDesigner.mChildNodeList != null)
            {
                for (int i = 0; i < nodeDesigner.mChildNodeList.Count; i++)
                {
                    var _child = nodeDesigner.mChildNodeList[i];
                    if (DrawNodeChildren(_child, offset, _child.mEditorNode.mDisable))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        
        #endregion
        //加载
        public void Load(BTEditorConfig _config)
        {
            mRootNode = BTEditorNodeFactory.CreateBTreeNodeDesignerFromConfig(_config.mRootNode)[0];
            mRootNode.SetEntryDisplay(true);
            if (_config.mDetachedNode != null)
            {
                mDetachedNodes = new List<BTNodeDesigner>();
                for (int i = 0; i < _config.mDetachedNode.Count; i++)
                {
                    BTNodeDesigner _detachedNode = BTEditorNodeFactory.CreateBTreeNodeDesignerFromConfig(_config.mDetachedNode[i])[0];
                    mDetachedNodes.Add(_detachedNode);
                }
            }
        }
        //获取鼠标位置上的节点
        public BTNodeDesigner NodeAt(Vector2 point, Vector2 offset)
        {
            for (int i = 0; i < mSelectedNodes.Count; i++)
            {
                if (mSelectedNodes[i].Contains(point, offset, false))
                {
                    return mSelectedNodes[i];
                }
            }
            BTNodeDesigner result;
            if (mRootNode != null)
            {
                result = nodeChildrenAt(mRootNode, point, offset);
                if (result != null)
                {
                    return result;
                }
            }
            for (int j = 0; j < mDetachedNodes.Count; j++)
            {
                if (mDetachedNodes[j] != null)
                {
                    result = nodeChildrenAt(mDetachedNodes[j], point, offset);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
        public BTNodeDesigner nodeChildrenAt(BTNodeDesigner nodeDesigner, Vector2 point, Vector2 offset)
        {
            if (nodeDesigner.Contains(point, offset, true))
            {
                return nodeDesigner;
            }
            if (nodeDesigner.IsParent)
            {
                if (nodeDesigner.mChildNodeList != null)
                {
                    for (int i = 0; i < nodeDesigner.mChildNodeList.Count; i++)
                    {
                        BTNodeDesigner result;
                        if (nodeDesigner.mChildNodeList[i] != null)
                        {
                            result = nodeChildrenAt(nodeDesigner.mChildNodeList[i], point, offset);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            return null;
        }
        //选中
        public void Select(BTNodeDesigner nodeDesigner)
        {
            mSelectedNodes.Add(nodeDesigner);
            nodeDesigner.Select();
        }
        //取消选中
        public void Deselect(BTNodeDesigner nodeDesigner)
        {
            mSelectedNodes.Remove(nodeDesigner);
            nodeDesigner.Deselect();
        }
        //取消所有选中
        public void ClearNodeSelection()
        {
            if (mSelectedNodes != null)
            {
                for (int i = 0; i < mSelectedNodes.Count; i++)
                {
                    mSelectedNodes[i].Deselect();
                }
                mSelectedNodes.Clear();
            }
        }
        //禁用节点
        public void DisableNodeSelection()
        {
            if (mSelectedNodes != null)
            {
                for (int i = 0; i < mSelectedNodes.Count; i++)
                {
                    SetNodeActive(mSelectedNodes[i], false);
                }
            }
        }
        //启用节点
        public void EnableNodeSelection()
        {
            if (mSelectedNodes != null)
            {
                for (int i = 0; i < mSelectedNodes.Count; i++)
                {
                    SetNodeActive(mSelectedNodes[i], true);
                }
            }
        }
        private void SetNodeActive(BTNodeDesigner nodeDesigner, bool isEnable)
        {
            if (isEnable)
            {
                nodeDesigner.Enable();
            }
            else
            {
                nodeDesigner.Disable();
            }
            if (nodeDesigner.mChildNodeList != null)
            {
                for (int i = 0; i < nodeDesigner.mChildNodeList.Count; i++)
                {
                    SetNodeActive(nodeDesigner.mChildNodeList[i], isEnable);
                }
            }
        }
        //删除选定节点
        public void DeleteSelectNode()
        {
            for (int i = 0; i < mSelectedNodes.Count; i++)
            {
                DeleteNode(mSelectedNodes[i]);
            }
            ClearNodeSelection();
        }
        //添加连线
        public void AddSelectNodeLine(BTNodeDesigner destNode)
        {
            if (mRootNode != null && mRootNode.Equals(destNode))
            {
                return;
            }
            if (mSelectedNodes != null && mSelectedNodes.Count == 1)
            {
                mSelectedNodes[0].AddChildNode(destNode);
            }
            if (mDetachedNodes != null)
            {
                for (int i = 0; i < mDetachedNodes.Count; i++)
                {
                    if (mDetachedNodes[i].Equals(destNode))
                    {
                        mDetachedNodes.RemoveAt(i);
                    }
                }
            }
        }
        //设置入口节点
        public void SetSelectNodeAsEntry()
        {
            if (mSelectedNodes != null && mSelectedNodes.Count == 1)
            {
                if (mRootNode != null)
                {
                    mRootNode.SetEntryDisplay(false);
                    mDetachedNodes.Add(mRootNode);
                }
                mRootNode = mSelectedNodes[0];
                mSelectedNodes[0].SetEntryDisplay(true);
            }
        }
        //拖动选择的节点
        public bool DragSelectedNodes(Vector2 delta, bool dragChildren, bool hasDragged)
        {
            if (mSelectedNodes.Count == 0)
            {
                return false;
            }
            for (int i = 0; i < mSelectedNodes.Count; i++)
            {
                DragTask(mSelectedNodes[i], delta, dragChildren, hasDragged);
            }
            return true;
        }
        private void DragTask(BTNodeDesigner nodeDesigner, Vector2 delta, bool dragChildren, bool hasDragged)
        {
            nodeDesigner.MovePosition(delta);
            if (nodeDesigner.IsParent && dragChildren)
            {
                for (int i = 0; i < nodeDesigner.mChildNodeList.Count; i++)
                {
                    DragTask(nodeDesigner.mChildNodeList[i], delta, dragChildren, hasDragged);
                }
            }
        }
        //添加节点
        public BTNodeDesigner AddNode(Type type, Vector2 position)
        {
            BTNode _node = (BTNode)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            BTEditorNode _editorNode = new BTEditorNode(_node);
            _editorNode.mPos = position;
            BTNodeDesigner _nodeDesigner = new BTNodeDesigner(_editorNode);
            if (mRootNode == null)
            {
                mRootNode = _nodeDesigner;
                _nodeDesigner.SetEntryDisplay(true);
            }
            else
            {
                if (mDetachedNodes == null)
                {
                    mDetachedNodes = new List<BTNodeDesigner>();
                }
                mDetachedNodes.Add(_nodeDesigner);
            }
            return _nodeDesigner;
        }
        //删除节点
        public void DeleteNode(BTNodeDesigner nodeDesigner)
        {
            if (nodeDesigner.IsParent)
            {
                for (int i = 0; i < nodeDesigner.mChildNodeConnectionList.Count; i++)
                {
                    BTNodeDesigner _destinationNodeDesigner = nodeDesigner.mChildNodeConnectionList[i].mDestinationNodeDesigner;
                    mDetachedNodes.Add(_destinationNodeDesigner);
                    _destinationNodeDesigner.mParentNode = null;
                }
            }
            if (nodeDesigner.mParentNode != null)
            {
                nodeDesigner.mParentNode.DeleteChildNode(nodeDesigner);
            }
            mDetachedNodes.Remove(nodeDesigner);
            if (mRootNode != null && mRootNode.Equals(nodeDesigner))
            {
                mRootNode = null;
            }
        }
    }
}
