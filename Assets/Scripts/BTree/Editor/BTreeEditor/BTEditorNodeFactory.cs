
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BTree.Editor
{
    class BTEditorNodeFactory
    {
        public static void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                Assembly assembly = null;
                try
                {
                    switch (i)
                    {
                        case 0:
                            assembly = Assembly.Load("Assembly-CSharp");
                            break;
                        case 1:
                            assembly = Assembly.Load("Assembly-CSharp-firstpass");
                            break;
                        case 2:
                            assembly = Assembly.Load("Assembly-UnityScript");
                            break;
                        case 3:
                            assembly = Assembly.Load("Assembly-UnityScript-firstpass");
                            break;
                    }
                }
                catch (Exception)
                {
                    assembly = null;
                }
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    for (int j = 0; j < types.Length; j++)
                    {
                        if (!types[j].IsAbstract)
                        {
                            if (types[j].IsSubclassOf(typeof(BTAction)))
                            {
                                BTFactory.RegisterActionType(types[j]);
                            }
                            else if (types[j].IsSubclassOf(typeof(BTPrecondition)))
                            {
                                BTFactory.RegisterPreconditionType(types[j]);
                            }
                        }
                    }
                }
            }
        }
        #region 从配置生成行为树编辑器相关方法
        public static BTNodeDesigner[] CreateBTreeNodeDesignerFromConfig(BTEditorTreeConfig _config)
        {
            BTNodeDesigner[] _nodeDesigners = new BTNodeDesigner[_config.mEditorNodes.Length];
            BTEditorNode[] _editorNodes = CreateBTreeEditorNode(_config);
            //递归创建节点
            for (int i = 0; i < _nodeDesigners.Length; i++)
            {
                if (_nodeDesigners[i] == null)
                {
                    _nodeDesigners[i] = CreateBTreeNodeDesigner(_config.mEditorNodes, _editorNodes, ref _nodeDesigners, i);
                }
            }
            //初始化父节点与连线
            for (int i = 0; i < _nodeDesigners.Length; i++)
            {
                var _editorNode = _editorNodes[i];
                if (_editorNode.mNode.parent != null)
                {
                    int _parentIndex = _editorNode.mNode.parent.index;
                    _nodeDesigners[i].mParentNode = _nodeDesigners[_parentIndex];
                    BTNodeConnection _connection = new BTNodeConnection(_nodeDesigners[i], _nodeDesigners[_parentIndex], NodeConnectionType.Incoming);
                    _nodeDesigners[i].mParentNodeConnection = _connection;
                }
            }
            return _nodeDesigners;
        }
        public static BTNodeDesigner CreateBTreeNodeDesigner(BTEditorNodeConfig[] _configNodes, BTEditorNode[] _editorNodes, ref BTNodeDesigner[] _nodeDesigners, int _index)
        {
            BTEditorNode _editorNode = _editorNodes[_index];
            for (int i = 0; i < _editorNode.mNode.childCount; i++)
            {
                int _childIndex = _editorNode.mNode.children[i].index;
                if (_nodeDesigners[_childIndex] == null)
                {
                    _nodeDesigners[_childIndex] = CreateBTreeNodeDesigner(_configNodes, _editorNodes, ref _nodeDesigners, _childIndex);
                }
            }
            BTNodeDesigner _node = new BTNodeDesigner(_editorNode);
            //_node.m_EditorNode = _editorNode;
            //_node.m_NodeName = _editorNode.m_Node.m_Name;
            //_node.m_ChildNodeList = new List<BTreeNodeDesigner>();
            //_node.m_ChildNodeConnectionList = new List<BTreeNodeConnection>();
            
            for (int i = 0; i < _editorNode.mNode.childCount; i++)
            {
                int _childIndex = _editorNode.mNode.children[i].index;
                _node.mChildNodeList.Add(_nodeDesigners[_childIndex]);
                BTNodeConnection _connection = new BTNodeConnection(_nodeDesigners[_childIndex], _node, NodeConnectionType.Outgoing);
                _node.mChildNodeConnectionList.Add(_connection);
            }
            return _node;
        }
        public static BTEditorNode[] CreateBTreeEditorNode(BTEditorTreeConfig _config)
        {
            BTNode[] _btreeNodes = BTFactory.CreateBTreeFromConfig(_config);
            BTEditorNode[] _editorNodes = new BTEditorNode[_btreeNodes.Length];
            for (int i = 0; i < _editorNodes.Length; i++)
            {
                _editorNodes[i] = new BTEditorNode(_btreeNodes[i]);
                _editorNodes[i].mPos = new Vector2(_config.mEditorNodes[i].mPosX, _config.mEditorNodes[i].mPosY);
                _editorNodes[i].mDisable = _config.mEditorNodes[i].mDisable;
            }
            return _editorNodes;
        }
        #endregion

        #region 从行为树编辑器类生成配置
        public static BTEditorConfig CreateBtreeEditorConfigFromGraphDesigner(BTGraphDesigner _graphDesigner)
        {
            BTEditorConfig _config = new BTEditorConfig();
            
            _config.mRootNode = CreateEditorTreeConfigFromRootEditorNode(_graphDesigner.mRootNode);
            _config.mRootNode.mIsEnterNode = true;

            _config.mDetachedNode = new List<BTEditorTreeConfig>();
            for (int i = 0; i < _graphDesigner.mDetachedNodes.Count; i++)
            {
                _config.mDetachedNode.Add(CreateEditorTreeConfigFromRootEditorNode(_graphDesigner.mDetachedNodes[i]));
            }
            return _config;
        }
        public static BTEditorTreeConfig CreateEditorTreeConfigFromRootEditorNode(BTNodeDesigner _rootEditorNode)
        {
            TreeConfig _treeConfig = BTFactory.CreateConfigFromBTreeRoot(_rootEditorNode.mEditorNode.mNode);
            BTEditorTreeConfig _treeEditorConfig = new BTEditorTreeConfig(_treeConfig);
            int index = 0;
            CreateEditorNodeConfigFromRootEditorNode(_rootEditorNode, ref _treeEditorConfig.mEditorNodes, ref index);
            
            return _treeEditorConfig;
        }
        public static BTEditorNodeConfig CreateEditorNodeConfigFromRootEditorNode(BTNodeDesigner _rootEditorNode, ref BTEditorNodeConfig[] _editorNodes, ref int _index)
        {
            //int _index = _rootEditorNode.m_EditorNode.m_Node.m_Index;
            _editorNodes[_index].mPosX = _rootEditorNode.mEditorNode.mPos.x;
            _editorNodes[_index].mPosY = _rootEditorNode.mEditorNode.mPos.y;
            _editorNodes[_index].mDisable = _rootEditorNode.mEditorNode.mDisable;
            if (_rootEditorNode.mChildNodeList != null)
            {
                for (int i = 0; i < _rootEditorNode.mChildNodeList.Count; i++)
                {
                    _index = _index + 1;
                    CreateEditorNodeConfigFromRootEditorNode(_rootEditorNode.mChildNodeList[i], ref _editorNodes, ref _index);
                }
            }
            return _editorNodes[_index];
        }
        #endregion
        #region 从行为树编辑器类生成运行时配置
        public static TreeConfig CreateTreeConfigFromBTreeGraphDesigner(BTGraphDesigner _graphDesigner)
        {
            BTNode _root = _graphDesigner.mRootNode.mEditorNode.mNode;
            return BTFactory.CreateConfigFromBTreeRoot(_root);
        }
        #endregion
    }
}
