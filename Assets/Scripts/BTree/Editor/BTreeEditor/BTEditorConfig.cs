
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BTree.Editor
{
    [Serializable]
    public class BTEditorConfig
    {
        public BTEditorTreeConfig mRootNode;
        public List<BTEditorTreeConfig> mDetachedNode;
        public BTEditorConfig()
        {
           
        }
        public BTEditorConfig(string xml)
        {
            mRootNode = new BTEditorTreeConfig(BTSerialization.ReadXML(xml));
            mRootNode.mIsEnterNode = true;
            mRootNode.mEditorNodes = new BTEditorNodeConfig[mRootNode.mNodes.Length];
            for (int i = 0; i < mRootNode.mEditorNodes.Length; i++)
            {
                mRootNode.mEditorNodes[i] = new BTEditorNodeConfig(mRootNode.mNodes[i]);
                mRootNode.mEditorNodes[i].mPosX = (i + 1) * 60;
                mRootNode.mEditorNodes[i].mPosY = (i + 1) * 60;
                mRootNode.mEditorNodes[i].mDisable = false;
            }
        }
    }
    [Serializable]
    public class BTEditorTreeConfig : TreeConfig
    {
        public BTEditorTreeConfig()
        {

        }
        public BTEditorTreeConfig(TreeConfig _config)
        {
            mNodes = _config.mNodes;
            mName = _config.mName;
            mEditorNodes = new BTEditorNodeConfig[mNodes.Length];
            for (int i = 0; i < mEditorNodes.Length; i++)
            {
                mEditorNodes[i] = new BTEditorNodeConfig(mNodes[i]);
            }
            mIsEnterNode = false;
        }
        public BTEditorNodeConfig[] mEditorNodes;
        public bool mIsEnterNode;

    }
    [Serializable]
    public class BTEditorNodeConfig : TreeNodeConfig
    {
        public BTEditorNodeConfig()
        {

        }
        public BTEditorNodeConfig(TreeNodeConfig _config)
        {
            mActionNodeName = _config.mActionNodeName;
            mNodeName = _config.mNodeName;
            mNodeSubType = _config.mNodeSubType;
            mNodeType = _config.mNodeType;
            mOtherParams = _config.mOtherParams;
            mParentIndex = _config.mParentIndex;
            mPreconditions = _config.mPreconditions;
        }
        public float mPosX;
        public float mPosY;
        public bool mDisable;
    }
}
