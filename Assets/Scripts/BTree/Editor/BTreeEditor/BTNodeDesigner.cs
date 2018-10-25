
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BTree.Editor
{
    public class BTNodeDesigner
    {
        public BTEditorNode mEditorNode;
        public BTNodeDesigner mParentNode;
        public List<BTNodeDesigner> mChildNodeList;
        public List<BTNodeConnection> mChildNodeConnectionList;
        public BTNodeConnection mParentNodeConnection;

        public string NodeName { get { return mEditorNode.mNode.name; } }

        private bool mIsSelected;
        private bool mIsDirty = true;
        private bool mIsShowHoverBar;
        public bool mIsEntryDisplay;
        public bool IsDisable
        {
            get
            {
                if (mEditorNode != null)
                {
                    return mEditorNode.mDisable;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsParent
        {
            get
            {
                if (mEditorNode != null)
                {
                    return mEditorNode.mNode.childCount != 0;
                }
                else
                {
                    return true;
                }
            }
        }

        private Texture mIcon;

        public BTNodeDesigner(BTEditorNode _editorNode)
        {
            if (_editorNode == null)
            {
                Debugger.Log("BTreeNodeDesigner Init Null");
                return;
            }
            mEditorNode = _editorNode;
            mChildNodeList = new List<BTNodeDesigner>();
            mChildNodeConnectionList = new List<BTNodeConnection>();
            LoadTaskIcon();
        }
        #region 节点操作方法
        public void Disable()
        {
            mEditorNode.mDisable = true;
        }
        public void Enable()
        {
            mEditorNode.mDisable = false;
        }
        public void Select()
        {
            mIsSelected = true;
        }
        public void Deselect()
        {
            mIsSelected = false;
        }
        public void DeleteChildNode(BTNodeDesigner childNodeDesigner)
        {
            mChildNodeList.Remove(childNodeDesigner);
            for (int i = 0; i < mChildNodeConnectionList.Count; i++)
            {
                if (mChildNodeConnectionList[i].mDestinationNodeDesigner.Equals(childNodeDesigner))
                {
                    mChildNodeConnectionList.RemoveAt(i);
                    mEditorNode.DelChildNode(childNodeDesigner.mEditorNode);
                    MarkDirty();
                    break;
                }
            }
            childNodeDesigner.mParentNode = null;
        }
        public void AddChildNode(BTNodeDesigner destNode)
        {
            BTNodeConnection line = new BTNodeConnection(destNode, this, NodeConnectionType.Outgoing);
            if (mChildNodeConnectionList == null)
            {
                mChildNodeConnectionList = new List<BTNodeConnection>();
            }
            for (int i = 0; i < mChildNodeConnectionList.Count; i++)
            {
                if (mChildNodeConnectionList[i].mDestinationNodeDesigner.Equals(destNode))
                {
                    return;
                }
            }
            mChildNodeConnectionList.Add(line);
            mChildNodeList.Add(destNode);
            mEditorNode.AddChildNode(destNode.mEditorNode);
            destNode.AddParentConnectionLine(this);
            MarkDirty();
        }
        public void AddParentConnectionLine(BTNodeDesigner orgNode)
        {
            BTNodeConnection line = new BTNodeConnection(this, orgNode, NodeConnectionType.Incoming);
            if (mParentNodeConnection != null)
            {
                mParentNodeConnection.mOriginatingNodeDesigner.DeleteChildNode(this);
            }
            mParentNodeConnection = line;
            mParentNode = orgNode;
            MarkDirty();
        }
        public void MovePosition(Vector2 delta)
        {
            Vector2 vector = mEditorNode.mPos;
            vector += delta;
            mEditorNode.mPos = vector;
            if (mParentNode != null)
            {
                mParentNode.MarkDirty();
            }
            MarkDirty();
        }
        public void MarkDirty()
        {
            mIsDirty = true;
        }
        public Rect IncomingConnectionRect(Vector2 offset)
        {
            Rect rect = RectAngle(offset, false);
            return new Rect(rect.x + (rect.width - BTEditorUtility.ConnectionWidth) / 2f, rect.y - BTEditorUtility.TopConnectionHeight, BTEditorUtility.ConnectionWidth, BTEditorUtility.TopConnectionHeight);
        }
        public Rect OutgoingConnectionRect(Vector2 offset)
        {
            Rect rect = RectAngle(offset, false);
            return new Rect(rect.x + (rect.width - BTEditorUtility.ConnectionWidth) / 2f, rect.yMax, BTEditorUtility.ConnectionWidth, BTEditorUtility.BottomConnectionHeight);
        }
        public void SetEntryDisplay(bool isEntry)
        {
            mIsEntryDisplay = isEntry;
        }
        #endregion
        #region 绘制方法相关
        //绘制节点
        public bool DrawNode(Vector2 offset, bool drawSelected, bool disabled)
        {
            Rect rect = RectAngle(offset, false);
            GUI.color = IsDisable ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
            //上部
            if (!mIsEntryDisplay)
            {
                GUI.DrawTexture(new Rect(rect.x + (rect.width - BTEditorUtility.ConnectionWidth) / 2f, rect.y - BTEditorUtility.TopConnectionHeight - BTEditorUtility.TaskBackgroundShadowSize + 4f, BTEditorUtility.ConnectionWidth, (BTEditorUtility.TopConnectionHeight + BTEditorUtility.TaskBackgroundShadowSize)), BTEditorUtility.TaskConnectionTopTexture, ScaleMode.ScaleToFit);
            }
            //下部
            if (mIsEntryDisplay || !mEditorNode.mNode.GetType().IsSubclassOf(typeof(BTAction)))
            {
                GUI.DrawTexture(new Rect(rect.x + (rect.width - BTEditorUtility.ConnectionWidth) / 2f, rect.yMax - 3f, BTEditorUtility.ConnectionWidth, (BTEditorUtility.BottomConnectionHeight + BTEditorUtility.TaskBackgroundShadowSize)), BTEditorUtility.TaskConnectionBottomTexture, ScaleMode.ScaleToFit);
            }
            //背景
            GUI.Label(rect, "", mIsSelected ? BTEditorUtility.TaskSelectedGUIStyle : BTEditorUtility.TaskGUIStyle);
            //图标背景
            GUI.DrawTexture(new Rect(rect.x + (rect.width - BTEditorUtility.IconBorderSize) / 2f, rect.y + ((BTEditorUtility.IconAreaHeight - BTEditorUtility.IconBorderSize) / 2) + 2f, BTEditorUtility.IconBorderSize, BTEditorUtility.IconBorderSize), BTEditorUtility.TaskBorderTexture);
            //图标
            GUI.DrawTexture(new Rect(rect.x + (rect.width - BTEditorUtility.IconSize) / 2f, rect.y + ((BTEditorUtility.IconAreaHeight - BTEditorUtility.IconSize) / 2) + 2f, BTEditorUtility.IconSize, BTEditorUtility.IconSize), mIcon);
            if (mIsShowHoverBar)
            {
                GUI.DrawTexture(new Rect(rect.x - 1f, rect.y - 17f, 14f, 14f), mEditorNode.mDisable ? BTEditorUtility.EnableTaskTexture : BTEditorUtility.DisableTaskTexture, ScaleMode.ScaleToFit);
                if (IsParent)
                {
                    GUI.DrawTexture(new Rect(rect.x + 15f, rect.y - 17f, 14f, 14f), mEditorNode.mIsCollapsed ? BTEditorUtility.ExpandTaskTexture : BTEditorUtility.CollapseTaskTexture, ScaleMode.ScaleToFit);
                }
            }
            GUI.Label(new Rect(rect.x, rect.yMax - BTEditorUtility.TitleHeight - 1f, rect.width, BTEditorUtility.TitleHeight), ToString(), BTEditorUtility.TaskTitleGUIStyle);
            return true;
        }
        //绘制连线
        public void DrawNodeConnection(Vector2 offset, float graphZoom, bool disabled)
        {
            if (mIsDirty)
            {
                DetermineConnectionHorizontalHeight(RectAngle(offset, false), offset);
                mIsDirty = false;
            }
            if (IsParent)
            {
                for (int i = 0; i < mChildNodeConnectionList.Count; i++)
                {
                    mChildNodeConnectionList[i].DrawConnection(offset, graphZoom, disabled);
                }
            }
        }
        //绘制节点说明
        public void DrawNodeComment(Vector2 offset)
        {

        }
        //获取连线位置
        public Vector2 GetConnectionPosition(Vector2 offset, NodeConnectionType connectionType)
        {
            Vector2 result;
            if (connectionType == NodeConnectionType.Incoming)
            {
                Rect rect = IncomingConnectionRect(offset);
                result = new Vector2(rect.center.x, rect.y + (BTEditorUtility.TopConnectionHeight / 2));
            }
            else
            {
                Rect rect2 = OutgoingConnectionRect(offset);
                result = new Vector2(rect2.center.x, rect2.yMax - (BTEditorUtility.BottomConnectionHeight / 2));
            }
            return result;
        }
        #endregion
        private void LoadTaskIcon()
        {
            Texture2D _icon = null;
            if (mEditorNode.mNode.GetType().IsSubclassOf(typeof(BTAction)))
            {
                _icon = BTEditorUtility.LoadTexture("ActionIcon.png");
            }
            else
            {
                Type type = mEditorNode.mNode.GetType();
                if (type == typeof(BTPrioritySelector))
                {
                    _icon = BTEditorUtility.PrioritySelectorIcon;
                }
                else if (type == typeof(BTNonePrioritySelector))
                {
                    _icon = BTEditorUtility.PrioritySelectorIcon;
                }
                else if (type == typeof(BTSequence))
                {
                    _icon = BTEditorUtility.SequenceIcon;

                }
                else if (type == typeof(BTParallel))
                {
                    _icon = BTEditorUtility.ParallelSelectorIcon;
                }
                else
                {
                    _icon = BTEditorUtility.InverterIcon;
                }
            }
            mIcon = _icon;
        }

        private Rect RectAngle(Vector2 offset, bool includeConnections)
        {
            Rect result = RectAngle(offset);
            if (includeConnections)
            {
                if (!mIsEntryDisplay)
                {
                    result.yMin = (result.yMin - BTEditorUtility.TopConnectionHeight);
                }
                if (IsParent)
                {
                    result.yMax = (result.yMax + BTEditorUtility.BottomConnectionHeight);
                }
            }
            return result;
        }
        private Rect RectAngle(Vector2 offset)
        {
            if (mEditorNode == null)
            {
                return default(Rect);
            }
            float num = BTEditorUtility.TaskTitleGUIStyle.CalcSize(new GUIContent(ToString())).x + BTEditorUtility.TextPadding;
            if (!IsParent)
            {
                float num2;
                float num3;
                BTEditorUtility.TaskCommentGUIStyle.CalcMinMaxWidth(new GUIContent("Comment(Test)"), out num2, out num3);
                num3 += BTEditorUtility.TextPadding;
                num = ((num > num3) ? num : num3);
            }
            num = Mathf.Min(BTEditorUtility.MaxWidth, Mathf.Max(BTEditorUtility.MinWidth, num));
            return new Rect(mEditorNode.mPos.x + offset.x - num / 2f, mEditorNode.mPos.y + offset.y, num, (BTEditorUtility.IconAreaHeight + BTEditorUtility.TitleHeight));
        }
        //确定连线横向高度
        private void DetermineConnectionHorizontalHeight(Rect nodeRect, Vector2 offset)
        {
            if (IsParent)
            {
                float num = float.MaxValue;
                float num2 = num;
                for (int i = 0; i < mChildNodeConnectionList.Count; i++)
                {
                    Rect rect = mChildNodeConnectionList[i].mDestinationNodeDesigner.RectAngle(offset, false);
                    if (rect.y < num)
                    {
                        num = rect.y;
                        num2 = rect.y;
                    }
                }
                num = num * 0.75f + nodeRect.yMax * 0.25f;
                if (num < nodeRect.yMax + 15f)
                {
                    num = nodeRect.yMax + 15f;
                }
                else if (num > num2 - 15f)
                {
                    num = num2 - 15f;
                }
                for (int j = 0; j < mChildNodeConnectionList.Count; j++)
                {
                    mChildNodeConnectionList[j].HorizontalHeight = num;
                }
            }
        }
        //是否包含坐标
        public bool Contains(Vector2 point, Vector2 offset, bool includeConnections)
        {
            return RectAngle(offset, includeConnections).Contains(point);
        }

        public override string ToString()
        {
            string isEntry = mIsEntryDisplay ? "(Entry)" : "";
            if (NodeName == null)
            {
                return isEntry;
            }
            string name = NodeName.Replace("BTreeNode", "");
            return name + isEntry;
        }
    }
}
