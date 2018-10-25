
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public class BTEditorGenericMenuBase
    {
        protected GenericMenu mMenu;
        protected BTEditorWindow mWindow;

        public BTEditorGenericMenuBase(BTEditorWindow _window)
        {
            mMenu = new GenericMenu();
            mWindow = _window;
        }

        public void AddDisabledItem(GUIContent content)
        {
            mMenu.AddDisabledItem(content);
        }

        public void AddItem(GUIContent content, bool on, GenericMenu.MenuFunction func)
        {
            mMenu.AddItem(content, on, func);
        }

        public void AddItem(GUIContent content, bool on, GenericMenu.MenuFunction2 func, object userData)
        {
            mMenu.AddItem(content, on, func, userData);
        }

        public void AddSeparator(string path)
        {
            mMenu.AddSeparator(path);
        }

        public void DropDown(Rect position)
        {
            mMenu.DropDown(position);
        }

        public int GetItemCount()
        {
            return mMenu.GetItemCount();
        }
        public virtual void ShowAsContext()
        {
            mMenu.ShowAsContext();
        }
    }
}
