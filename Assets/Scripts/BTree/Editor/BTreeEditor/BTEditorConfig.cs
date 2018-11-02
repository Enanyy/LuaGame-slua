
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
