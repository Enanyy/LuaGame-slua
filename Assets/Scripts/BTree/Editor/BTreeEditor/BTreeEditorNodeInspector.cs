
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorNodeInspector
    {
        private UnityEngine.Object[] m_Precondition = new UnityEngine.Object[20];

        private const int SPACEDETLE = 10;

        public void drawInspector(BTreeNodeDesigner _selectNode)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Script:", new GUILayoutOption[] { GUILayout.Width(100) });
            string[] scripts = AssetDatabase.FindAssets("t:Script " + _selectNode.m_EditorNode.m_Node.GetType().Name);
            if (scripts != null &&scripts.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(scripts[0]);
                MonoScript monoScript = (MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                EditorGUILayout.ObjectField("", monoScript, typeof(MonoScript), false);
            }
            GUILayout.EndHorizontal();

            //if (GUILayout.Button(BTreeEditorUtility.GearTexture, BTreeEditorUtility.TransparentButtonGUIStyle, new GUILayoutOption[0]))
            //{
            //    GenericMenu genericMenu = new GenericMenu();
            //    genericMenu.AddItem(new GUIContent("Edit Script"), false, new GenericMenu.MenuFunction2(openInFileEditor), _selectNode.m_EditorNode.m_Node);
            //    //genericMenu.AddItem(new GUIContent("Reset"), false, new GenericMenu.MenuFunction2(this.resetTask), _selectNode);
            //    genericMenu.ShowAsContext();
            //}
            var _node = _selectNode.m_EditorNode.m_Node;
            Type _nodeType = _selectNode.m_EditorNode.m_Node.GetType();
            FieldInfo[] fields = _nodeType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = fields.Length-1; i >= 0; i--)
            {
                DrawValue(_node, fields[i]);
            }
            GUILayout.Label("Precondition:");
            int index = -1;
            _node.SetPrecondition(DrawPrecondition(_node.precondition, 0, ref index));
            GUILayout.FlexibleSpace();
        }

        BTPrecondition DrawPrecondition(BTPrecondition _condition, int _space, ref int index)
        {
            index = index + 1;
            BTPrecondition result = null;
            if (_condition == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(_space);
                
                m_Precondition[index] = EditorGUILayout.ObjectField("", m_Precondition[index], typeof(MonoScript), false);
                if (m_Precondition[index] != null)
                {
                    Type type = GetPreconditionType(m_Precondition[index].name);
                    if (type == null)
                    {
                        m_Precondition[index] = null;
                        return _condition;
                    }
                    result = (BTPrecondition)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                m_Precondition[index] = null;
                GUILayout.BeginHorizontal();
                GUILayout.Space(_space);
                string[] scripts = AssetDatabase.FindAssets("t:Script " + _condition.GetType().Name);
                if (scripts.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(scripts[0]);
                    MonoScript monoScript = (MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                    var obj = EditorGUILayout.ObjectField("", monoScript, typeof(MonoScript), false);
                    Type type = GetPreconditionType(obj.name);
                    if (type == null)
                    {
                        return _condition;
                    }
                    result = (BTPrecondition)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                GUILayout.EndHorizontal();
                _space = _space + SPACEDETLE;

                BTPrecondition _lastPreCondition = _condition;
                if (result is BTPreconditionAND)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(_space);
                    int childCount = 0;
                    if (_lastPreCondition is BTPreconditionAND)
                    {
                        BTPreconditionAND _lastCondAND = (BTPreconditionAND)_lastPreCondition;
                        childCount = _lastCondAND.GetChildPreconditionCount();
                    }
                    var val = EditorGUILayout.IntField(childCount);
                    GUILayout.EndHorizontal();

                    if (_lastPreCondition is BTPreconditionAND)
                    {
                        BTPreconditionAND _lastCondAND = (BTPreconditionAND)_lastPreCondition;
                        BTPrecondition[] childPreconditions = new BTPrecondition[val];
                        BTPrecondition[] curChildPreconditions = _lastCondAND.GetChildPrecondition();
                        for (int i = 0; i < val; i++)
                        {
                            BTPrecondition _cond = null;
                            if (curChildPreconditions.Length >= i + 1)
                            {
                                _cond = curChildPreconditions[i];
                            }
                            childPreconditions[i] = DrawPrecondition(_cond, _space, ref index);
                        }
                        ((BTPreconditionAND)result).SetChildPrecondition(childPreconditions);
                    }
                    else
                    {
                        BTPrecondition[] childPreconditions = new BTPrecondition[val];
                        for (int i = 0; i < val; i++)
                        {
                            childPreconditions[i] = DrawPrecondition(null, _space, ref index);
                        }
                        ((BTPreconditionAND)result).SetChildPrecondition(childPreconditions);
                    }

                }
                else if (result is BTPreconditionOR)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(_space);
                    int childCount = 0;
                    if (_lastPreCondition is BTPreconditionOR)
                    {
                        BTPreconditionOR _lastCondOR = (BTPreconditionOR)_lastPreCondition;
                        childCount = _lastCondOR.GetChildPreconditionCount();
                    }
                    var val = EditorGUILayout.IntField(childCount);
                    GUILayout.EndHorizontal();

                    if (_lastPreCondition is BTPreconditionOR)
                    {
                        BTPreconditionOR _lastCondOR = (BTPreconditionOR)_lastPreCondition;
                        BTPrecondition[] childPreconditions = new BTPrecondition[val];
                        BTPrecondition[] curChildPreconditions = _lastCondOR.GetChildPrecondition();
                        for (int i = 0; i < val; i++)
                        {
                            BTPrecondition _cond = null;
                            if (curChildPreconditions.Length >= i + 1)
                            {
                                _cond = curChildPreconditions[i];
                            }
                            childPreconditions[i] = DrawPrecondition(_cond, _space, ref index);
                        }
                        ((BTPreconditionOR)result).SetChildPrecondition(childPreconditions);
                    }
                    else
                    {
                        BTPrecondition[] childPreconditions = new BTPrecondition[val];
                        for (int i = 0; i < val; i++)
                        {
                            childPreconditions[i] = DrawPrecondition(null, _space, ref index);
                        }
                        ((BTPreconditionOR)result).SetChildPrecondition(childPreconditions);
                    }
                }
                else if (result is BTPreconditionNOT)
                {
                    BTPrecondition curChildPreconditions = null;
                    if (_lastPreCondition is BTPreconditionNOT)
                    {
                        BTPreconditionNOT _lastCondNOT = (BTPreconditionNOT)_lastPreCondition;
                        curChildPreconditions = _lastCondNOT.GetChildPrecondition();
                    }
                    curChildPreconditions = DrawPrecondition(curChildPreconditions, _space, ref index);
                    ((BTPreconditionNOT)result).SetChildPrecondition(curChildPreconditions);
                }
            }
            
            return result;
        }
        void DrawValue(BTNode _node, FieldInfo _field)
        {
            if (_field == null)
            {
                return;
            }
            try
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(BTreeEditorUtility.SplitCamelCase(_field.Name)+":", new GUILayoutOption[] { GUILayout.Width(100) });
                if (_field.FieldType == typeof(int))
                {
                    var _val = EditorGUILayout.IntField((int)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(float))
                {
                    var _val = EditorGUILayout.FloatField((float)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(double))
                {
                    var _val = EditorGUILayout.DoubleField((float)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(string))
                {
                    var _val = EditorGUILayout.TextField((string)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType.IsEnum)
                {
                    var _val = EditorGUILayout.EnumPopup((Enum)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(Vector2))
                {
                    var _val = EditorGUILayout.Vector2Field("",(Vector2)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(Vector3))
                {
                    var _val = EditorGUILayout.Vector3Field("", (Vector3)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(Vector4))
                {
                    var _val = EditorGUILayout.Vector4Field("", (Vector4)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                GUILayout.EndHorizontal();
            }
            catch (Exception e)
            {
                Debugger.LogWarning(e.Message);
            }
        }
        private void openInFileEditor(object _node)
        {
            Debugger.Log(_node.GetType().Name);
            
        }
        private Type GetPreconditionType(string name)
        {
            Type type = null;
            if (name == typeof(BTPreconditionAND).Name)
            {
                type = typeof(BTPreconditionAND);
            }
            else if (name == typeof(BTPreconditionOR).Name)
            {
                type = typeof(BTPreconditionOR);
            }
            else if (name == typeof(BTPreconditionNOT).Name)
            {
                type = typeof(BTPreconditionNOT);
            }
            else
            {
                if (BTFactory.PreconditionTypeDic.ContainsKey(name))
                {
                    type = BTFactory.PreconditionTypeDic[name];
                }
            }
            return type;
        }
    }
}
