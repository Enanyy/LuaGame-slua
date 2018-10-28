
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace BTree.Editor
{
    public class BTEditorRightClickBlockMenu : BTEditorGenericMenuBase
    {
        public BTEditorRightClickBlockMenu(BTEditorWindow _window)
            : base(_window)
        {
            Init();
        }
        public void Init()
        {
            List<Type> actionList = new List<Type>();
            List<Type> compositeList = new List<Type>();
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
                                actionList.Add(types[j]);
                            }
                            else if (types[j].IsSubclassOf(typeof(BTComposite)))
                            {
                                compositeList.Add(types[j]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < actionList.Count; i++)
            {
                AddItem(new GUIContent("Add Task/Action/" + actionList[i].Name), false, new GenericMenu.MenuFunction2(AddNodeCallback), actionList[i]);
            }
            for (int i = 0; i < compositeList.Count; i++)
            {
                AddItem(new GUIContent("Add Task/Composite/" + compositeList[i].Name), false, new GenericMenu.MenuFunction2(AddNodeCallback), compositeList[i]);
            }
        }

        private void AddNodeCallback(object obj)
        {
            mWindow.AddNodeCallback(obj);
        }
        
    }
}
