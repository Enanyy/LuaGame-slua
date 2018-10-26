using UnityEditor;
using UnityEngine;
using System;

namespace BTree.Editor
{
    public class BTEditorWindow : EditorWindow
    {
        public static BTEditorWindow Instance;
        
        [MenuItem("Window/BTree Editor %#_t")]
        public static void ShowWindow()
        {
            BTEditorWindow bTreeEditorWindow = GetWindow(typeof(BTEditorWindow)) as BTEditorWindow;
            bTreeEditorWindow.titleContent = new GUIContent("行为树编辑器");
            bTreeEditorWindow.mIsFirst = true;
            bTreeEditorWindow.wantsMouseMove = true;
            bTreeEditorWindow.minSize = new Vector2(600f, 500f);
            DontDestroyOnLoad(bTreeEditorWindow);
        }
        private BTGraphDesigner _mGraphDesigner;
        private BTGraphDesigner mGraphDesigner
        {
            get
            {
                if (_mGraphDesigner == null)
                {
                    _mGraphDesigner = new BTGraphDesigner();
                }
                return _mGraphDesigner;
            }
            set
            {
                _mGraphDesigner = value;
            }
        }

        private Rect mGraphRect;
        private Rect mFileToolBarRect;
        private Rect mPropertyToolbarRect;
        private Rect mPropertyBoxRect;
        private Rect mPreferencesPaneRect;

        private Vector2 mCurrentMousePosition = Vector2.zero;

        private Vector2 mGraphScrollSize = new Vector2(10000f, 10000f);
        private Vector2 mGraphScrollPosition = new Vector2(-1f, -1f);
        private Vector2 mGraphOffset = Vector2.zero;
        private float mGraphZoom = 1f;

        //是否显示设置界面
        private bool mShowPrefPanel;
        //是否点击节点中
        private bool mNodeClicked;
        //是否在拖动中
        private bool mIsDragging;
        //是否在连线状态
        private bool mIsConnectingLine;
        //当前鼠标位置的节点
        private BTNodeDesigner mCurMousePosNode;

        //右键菜单
        private BTEditorRightClickBlockMenu mRightClickBlockMenu = null;
        private BTEditorRightClickNodeMenu mRightClickNodeMenu = null;

        //属性栏
        private BTEditorNodeInspector mNodeInspector = new BTEditorNodeInspector();

        private bool mIsFirst;

        public void OnGUI()
        {
            if (mIsFirst)
            {
                mIsFirst = false;
            }
            mCurrentMousePosition = Event.current.mousePosition;
            SetupSizes();
            HandleEvents();
            if (Draw())
            {
                Repaint();
            }
        }

        public bool Draw()
        {
            bool result = false;
            Color color = GUI.color;
            Color backgroundColor = GUI.backgroundColor;
            GUI.color = (Color.white);
            GUI.backgroundColor = (Color.white);
            DrawFileToolbar();
            DrawPropertiesBox();
            if (DrawGraphArea())
            {
                result = true;
            }
            GUI.color = color;
            GUI.backgroundColor = backgroundColor;
            return result;
        }
        private void SetupSizes()
        {

            mFileToolBarRect = new Rect(BTEditorUtility.PropertyBoxWidth, 0f, (Screen.width - BTEditorUtility.PropertyBoxWidth), BTEditorUtility.ToolBarHeight);
            mPropertyToolbarRect = new Rect(0f, 0f, BTEditorUtility.PropertyBoxWidth, BTEditorUtility.ToolBarHeight);
            mPropertyBoxRect = new Rect(0f, mPropertyToolbarRect.height, BTEditorUtility.PropertyBoxWidth, Screen.height - mPropertyToolbarRect.height - BTEditorUtility.EditorWindowTabHeight);
            mGraphRect = new Rect(BTEditorUtility.PropertyBoxWidth, BTEditorUtility.ToolBarHeight, (Screen.width - BTEditorUtility.PropertyBoxWidth - BTEditorUtility.ScrollBarSize), (Screen.height - BTEditorUtility.ToolBarHeight - BTEditorUtility.EditorWindowTabHeight - BTEditorUtility.ScrollBarSize));
            mPreferencesPaneRect = new Rect(BTEditorUtility.PropertyBoxWidth + mGraphRect.width - BTEditorUtility.PreferencesPaneWidth, (BTEditorUtility.ToolBarHeight + (EditorGUIUtility.isProSkin ? 1 : 2)), BTEditorUtility.PreferencesPaneWidth, BTEditorUtility.PreferencesPaneHeight);

            if (mGraphScrollPosition == new Vector2(-1f, -1f))
            {
                mGraphScrollPosition = (mGraphScrollSize - new Vector2(mGraphRect.width, mGraphRect.height)) / 2f - 2f * new Vector2(BTEditorUtility.ScrollBarSize, BTEditorUtility.ScrollBarSize);
            }
        }
        //绘制图形区域
        private bool DrawGraphArea()
        {
            Vector2 vector = GUI.BeginScrollView(new Rect(mGraphRect.x, mGraphRect.y, mGraphRect.width + BTEditorUtility.ScrollBarSize, mGraphRect.height + BTEditorUtility.ScrollBarSize), mGraphScrollPosition, new Rect(0f, 0f, mGraphScrollSize.x, mGraphScrollSize.y), true, true);
            if (vector != mGraphScrollPosition && Event.current.type != EventType.DragUpdated && Event.current.type != EventType.Ignore)
            {
                mGraphOffset -= (vector - mGraphScrollPosition) / mGraphZoom;
                mGraphScrollPosition = vector;
                //mGraphDesigner.graphDirty();
            }
            GUI.EndScrollView();
            GUI.Box(mGraphRect, "", BTEditorUtility.GraphBackgroundGUIStyle);

            BTEditorZoomArea.Begin(mGraphRect, mGraphZoom);
            Vector2 mousePosition;
            if (!GetMousePositionInGraph(out mousePosition))
            {
                mousePosition = new Vector2(-1f, -1f);
            }
            bool result = false;
            if (mGraphDesigner.DrawNodes(mousePosition, mGraphOffset, mGraphZoom))
            {
                result = true;
            }
            if (mIsConnectingLine)
            {
                var _curNode = mGraphDesigner.NodeAt(mousePosition, mGraphOffset);
                Vector2 des = _curNode == null ? mousePosition : _curNode.mEditorNode.mPos;
                mGraphDesigner.DrawTempConnection(des, mGraphOffset, mGraphZoom);
            }
            BTEditorZoomArea.End();
            return result;
        }
        //绘制工具栏
        private void DrawFileToolbar()
        {
            GUILayout.BeginArea(mFileToolBarRect, EditorStyles.toolbar);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("New", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(42f)
            }))
            {
                NewBTree();
            }
            if (GUILayout.Button("Load", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(42f)
            }))
            {
                LoadBTree();
            }
            if (GUILayout.Button("Save", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(42f)
            }))
            {
                SaveBTree();
            }
            if (GUILayout.Button("Export XML", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                ExportXMLBTree();
            }
            if (GUILayout.Button("Export Binary", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                ExportBinaryBTree();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Preferences", mShowPrefPanel ? BTEditorUtility.ToolbarButtonSelectionGUIStyle : EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                mShowPrefPanel = !mShowPrefPanel;
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //绘制属性栏
        private void DrawPropertiesBox()
        {
            GUILayout.BeginArea(mPropertyToolbarRect, EditorStyles.toolbar);
            GUILayout.EndArea();
            GUILayout.BeginArea(mPropertyBoxRect, BTEditorUtility.PropertyBoxGUIStyle);
            if (mGraphDesigner.mSelectedNodes.Count == 1)
            {
                mNodeInspector.DrawInspector(mGraphDesigner.mSelectedNodes[0]);
            }
            GUILayout.EndArea();
        }
        
        #region 操作消息处理相关
        //获取鼠标位置是否在绘图区域内
        private bool GetMousePositionInGraph(out Vector2 mousePosition)
        {
            mousePosition = mCurrentMousePosition;
            if (!mGraphRect.Contains(mousePosition))
            {
                return false;
            }
            if (mShowPrefPanel && mPreferencesPaneRect.Contains(mousePosition))
            {
                return false;
            }
            mousePosition -= new Vector2(mGraphRect.xMin, mGraphRect.yMin);
            mousePosition /= mGraphZoom;
            return true;
        }
        //处理操作消息
        private void HandleEvents()
        {
            if (EditorApplication.isCompiling) return;

            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (LeftMouseDown(e.clickCount))
                        {
                            e.Use();
                            return;
                        }
                    }
                    else if (e.button == 1)
                    {
                        if (RightMouseDown())
                        {
                            e.Use();
                            return;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (e.button == 0)
                    {
                        if (LeftMouseRelease())
                        {
                            e.Use();
                            return;
                        }
                    }
                    else if (e.button == 1)
                    {
                        if (RightMouseRelease())
                        {
                            e.Use();
                            return;
                        }
                    }
                    break;
                case EventType.MouseMove:
                    if (MouseMove())
                    {
                        e.Use();
                        return;
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        if (LeftMouseDragged())
                        {
                            e.Use();
                            return;
                        }
                        if (e.modifiers == EventModifiers.Alt && MousePan())
                        {
                            e.Use();
                            return;
                        }
                    }
                    else if (e.button == 2 && MousePan())
                    {
                        e.Use();
                        return;
                    }
                    break;
                case EventType.KeyDown:
                    break;
                case EventType.KeyUp:
                    break;
                case EventType.ScrollWheel:
                    if (MouseZoom())
                    {
                        e.Use();
                        return;
                    }
                    break;
                case EventType.Repaint:
                    break;
                case EventType.Layout:
                    break;
                case EventType.DragUpdated:
                    break;
                case EventType.DragPerform:
                    break;
                case EventType.DragExited:
                    break;
                case EventType.Ignore:
                    break;
                case EventType.Used:
                    break;
                case EventType.ValidateCommand:
                    break;
                case EventType.ExecuteCommand:
                    break;
                case EventType.ContextClick:
                    break;
                default:
                    break;
            }
        }
        //鼠标移动
        private bool MouseMove()
        {
            return true;
        }
        //鼠标左键down
        private bool LeftMouseDown(int clickCount)
        {
            Vector2 point;
            if (!GetMousePositionInGraph(out point))
            {
                mIsConnectingLine = false;
                return false;
            }
            var nodeDesigner = mGraphDesigner.NodeAt(point, mGraphOffset);
            if (mIsConnectingLine && nodeDesigner != null)
            {
                mGraphDesigner.AddSelectNodeLine(nodeDesigner);
            }
            mGraphDesigner.ClearNodeSelection();
            if (nodeDesigner != null)
            {
                mGraphDesigner.Select(nodeDesigner);
                mNodeClicked = true;
            }
            mIsConnectingLine = false;
            return true;
        }
        private bool LeftMouseDragged()
        {
            Vector2 point;
            if (!GetMousePositionInGraph(out point))
            {
                return false;
            }
            if (mNodeClicked)
            {
                bool flag = mGraphDesigner.DragSelectedNodes(Event.current.delta / mGraphZoom, Event.current.modifiers != EventModifiers.Alt, mIsDragging);
                if (flag)
                {
                    mIsDragging = true;
                }
            }
            return true;
        }
        //鼠标左键Release
        private bool LeftMouseRelease()
        {
            Vector2 point;
            if (!GetMousePositionInGraph(out point))
            {
                return false;
            }
            mNodeClicked = false;
            return true;
        }
        //鼠标右键down
        private bool RightMouseDown()
        {
            Vector2 point;
            mIsConnectingLine = false;
            if (!GetMousePositionInGraph(out point))
            {
                return false;
            }
            mGraphDesigner.ClearNodeSelection();
            var nodeDesigner = mGraphDesigner.NodeAt(point, mGraphOffset);
            if (nodeDesigner != null)
            {
                mGraphDesigner.Select(nodeDesigner);
                mNodeClicked = true;
            }
            return true;
        }
        //鼠标右键Release
        private bool RightMouseRelease()
        {
            Vector2 point;
            if (!GetMousePositionInGraph(out point))
            {
                return false;
            }
            if (mGraphDesigner.mSelectedNodes != null && mGraphDesigner.mSelectedNodes.Count != 0)
            {
                if (mRightClickNodeMenu == null)
                {
                    mRightClickNodeMenu = new BTEditorRightClickNodeMenu(this);
                }
                mRightClickNodeMenu.ShowAsContext(mGraphDesigner.mSelectedNodes);
                return true;
            }
            else
            {
                if (mRightClickBlockMenu == null)
                {
                    mRightClickBlockMenu = new BTEditorRightClickBlockMenu(this);
                }
                mRightClickBlockMenu.ShowAsContext();
                return true;
            }
        }
        private bool MouseZoom()
        {
            Vector2 point;
            if (!GetMousePositionInGraph(out point))
            {
                return false;
            }
            return true;
        }
        private bool MousePan()
        {
            Vector2 point;
            if (!GetMousePositionInGraph(out point))
            {
                return false;
            }
            return true;
        }
        //添加节点
        private void AddNode(Type type, bool useMousePosition)
        {
            Vector2 vector = new Vector2(mGraphRect.width / (2f * mGraphZoom), 150f);
            if (useMousePosition)
            {
                GetMousePositionInGraph(out vector);
            }
            vector -= mGraphOffset;
            if (mGraphDesigner.AddNode(type, vector) != null)
            {
                Debugger.Log("addNode");
            }
        }
        //禁用节点
        private void DisableSelectNode()
        {
            mGraphDesigner.DisableNodeSelection();
        }
        //启用节点
        private void EnableSelectNode()
        {
            mGraphDesigner.EnableNodeSelection();
        }
        //删除节点
        private void DeleteSelectNode()
        {
            mGraphDesigner.DeleteSelectNode();
        }
        //设置入口节点
        private void SetSelectNodeAsEntry()
        {
            mGraphDesigner.SetSelectNodeAsEntry();
        }
        #endregion

        #region 配置文件相关
        public void NewBTree()
        {
            Debugger.Log("createBTree");
            mGraphDesigner = null;
        }

        public void LoadBTree()
        {
            string text = EditorUtility.OpenFilePanel("Load Behavior Tree", BTEditorUtility.editorPath+ "Config", "xml");
            if (!string.IsNullOrEmpty(text))
            {
                Debugger.Log("loadBTree");
                BTEditorConfig _config = BTEditorSerialization.ReadXMLAtPath(text);
                mGraphDesigner = (new BTGraphDesigner());
                mGraphDesigner.Load(_config);
            }
        }
        public void SaveBTree()
        {
            if (mGraphDesigner == null || mGraphDesigner.mRootNode == null)
            {
                EditorUtility.DisplayDialog("Save Error", "未创建根节点", "ok");
                return;
            }
            string text = EditorUtility.SaveFilePanel("Save Behavior Tree", BTEditorUtility.editorPath + "Config", mGraphDesigner.mRootNode.NodeName,"xml");
            if (text.Length != 0 && Application.dataPath.Length < text.Length)
            {
                Debugger.Log("saveBTree");
                BTEditorConfig _config = BTEditorNodeFactory.CreateBtreeEditorConfigFromGraphDesigner(mGraphDesigner);
                BTEditorSerialization.WirteXMLAtPath(_config, text);
                EditorUtility.DisplayDialog("Save", "保存行为树编辑器成功:" + text, "ok");
            }
        }
        public void ExportXMLBTree()
        {
            if (mGraphDesigner == null || mGraphDesigner.mRootNode == null)
            {
                EditorUtility.DisplayDialog("Export Error", "未创建根节点", "ok");
                return;
            }
            string text = EditorUtility.SaveFilePanel("Save Behavior Tree", BTSerialization.configPath, mGraphDesigner.mRootNode.NodeName, "xml");
            if (text.Length != 0 && Application.dataPath.Length < text.Length)
            {
                Debugger.Log("exportBtree");
                TreeConfig _treeConfig = BTEditorNodeFactory.CreateTreeConfigFromBTreeGraphDesigner(mGraphDesigner);
               
                BTSerialization.WriteXML(_treeConfig,System.IO.Path.GetFileName(text));
                EditorUtility.DisplayDialog("Export", "导出行为树配置成功:" + text, "ok");
            }
        }
        public void ExportBinaryBTree()
        {
            if (mGraphDesigner == null || mGraphDesigner.mRootNode == null)
            {
                EditorUtility.DisplayDialog("Export Error", "未创建根节点", "ok");
                return;
            }
            Debugger.Log("exportBtree");
            TreeConfig _treeConfig = BTEditorNodeFactory.CreateTreeConfigFromBTreeGraphDesigner(mGraphDesigner);
            string name = mGraphDesigner.mRootNode.NodeName;
            BTSerialization.WriteBinary(_treeConfig, name);
            EditorUtility.DisplayDialog("Export", "导出行为树配置成功:" + name, "ok");
        }
        #endregion
        #region 右键菜单点击回调
        public void AddNodeCallback(object node)
        {
            AddNode((Type)node,true);
        }
        public void DisableNodeCallback()
        {
            DisableSelectNode();
        }
        public void EnableNodeCallback()
        {
            EnableSelectNode();
        }
        public void DelectNodeCallback()
        {
            DeleteSelectNode();
        }
        public void ConnectLineCallback()
        {
            mIsConnectingLine = true;
        }
        public void SetEntryNodeCallback()
        {
            SetSelectNodeAsEntry();
        }
        #endregion
    }
}
