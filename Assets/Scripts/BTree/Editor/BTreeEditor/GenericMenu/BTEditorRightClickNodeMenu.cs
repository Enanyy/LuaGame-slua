
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public class BTEditorRightClickNodeMenu : BTEditorGenericMenuBase
    {
        public BTEditorRightClickNodeMenu(BTEditorWindow _window)
            :base(_window)
        {
        }
        
        public void ShowAsContext(List<BTNodeDesigner> _selectNodes)
        {
            bool isMult = _selectNodes.Count != 1;
            bool isDisable = _selectNodes[0].IsDisable;
            bool isAction = _selectNodes[0].mEditorNode.mNode.GetType().IsSubclassOf(typeof(BTAction));
            bool isEntry = _selectNodes[0].mParentNode == null && !_selectNodes[0].mIsEntryDisplay;
            mMenu = new GenericMenu();
            if (!isMult)
            {
                if (!isAction)
                {
                    AddItem(new GUIContent("Make Transition"), false, new GenericMenu.MenuFunction(ConnectionCallback));
                }
                if (isEntry)
                {
                    AddItem(new GUIContent("Set As EntryNode"), false, new GenericMenu.MenuFunction(SetEntryNodeCallback));
                }
                if (isDisable)
                {
                    AddItem(new GUIContent("Set Enable"), false, new GenericMenu.MenuFunction(EnableCallback));
                }
                else
                {
                    AddItem(new GUIContent("Set Disable"), false, new GenericMenu.MenuFunction(DisableCallback));
                }
            }
            AddItem(new GUIContent("Delete"), false, new GenericMenu.MenuFunction(DeleteCallback));
            base.ShowAsContext();
        }

        private void DisableCallback()
        {
            mWindow.DisableNodeCallback();
        }
        private void EnableCallback()
        {
            mWindow.EnableNodeCallback();
        }
        private void DeleteCallback()
        {
            mWindow.DelectNodeCallback();
        }
        private void ConnectionCallback()
        {
            mWindow.ConnectLineCallback();
        }
        private void SetEntryNodeCallback()
        {
            mWindow.SetEntryNodeCallback();
        }
    }
}
