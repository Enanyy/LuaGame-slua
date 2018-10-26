
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public static class BTEditorUtility
    {
        public static readonly string editorPath = "Assets/Scripts/BTree/Editor/BTreeEditor/";

        public static readonly int ToolBarHeight = 18;

        public static readonly int PropertyBoxWidth = 320;

        public static readonly int ScrollBarSize = 15;

        public static readonly int EditorWindowTabHeight = 21;

        public static readonly int PreferencesPaneWidth = 290;

        public static readonly int PreferencesPaneHeight = 208;

        public static readonly float GraphZoomMax = 1f;

        public static readonly float GraphZoomMin = 0.4f;

        public static readonly float GraphZoomSensitivity = 150f;

        public static readonly int LineSelectionThreshold = 7;

        public static readonly int TaskBackgroundShadowSize = 3;

        public static readonly int TitleHeight = 20;

        public static readonly int IconAreaHeight = 52;

        public static readonly int IconSize = 44;

        public static readonly int IconBorderSize = 46;

        public static readonly int ConnectionWidth = 42;

        public static readonly int TopConnectionHeight = 14;

        public static readonly int BottomConnectionHeight = 16;

        public static readonly int TaskConnectionCollapsedWidth = 26;

        public static readonly int TaskConnectionCollapsedHeight = 6;

        public static readonly int MinWidth = 100;

        public static readonly int MaxWidth = 220;

        public static readonly int MaxCommentHeight = 100;

        public static readonly int TextPadding = 20;

        public static readonly float NodeFadeDuration = 0.5f;

        public static readonly int IdentifyUpdateFadeTime = 500;

        public static readonly int MaxIdentifyUpdateCount = 2000;

        public static readonly int TaskPropertiesLabelWidth = 150;

        public static readonly int MaxTaskDescriptionBoxWidth = 400;

        public static readonly int MaxTaskDescriptionBoxHeight = 300;

        public static readonly int LineWidth = 2;

        private static GUIStyle graphStatusGUIStyle = null;

        private static GUIStyle taskFoldoutGUIStyle = null;

        private static GUIStyle taskTitleGUIStyle = null;

        private static GUIStyle taskGUIStyle = null;

        private static GUIStyle taskSelectedGUIStyle = null;

        private static GUIStyle taskRunningGUIStyle = null;

        private static GUIStyle taskRunningSelectedGUIStyle = null;

        private static GUIStyle taskIdentifyGUIStyle = null;

        private static GUIStyle taskIdentifySelectedGUIStyle = null;

        private static GUIStyle taskCommentGUIStyle = null;

        private static GUIStyle taskCommentLeftAlignGUIStyle = null;

        private static GUIStyle taskCommentRightAlignGUIStyle = null;

        private static GUIStyle graphBackgroundGUIStyle = null;

        private static GUIStyle selectionGUIStyle = null;

        private static GUIStyle labelWrapGUIStyle = null;

        private static GUIStyle taskInspectorCommentGUIStyle = null;

        private static GUIStyle taskInspectorGUIStyle = null;

        private static GUIStyle toolbarButtonSelectionGUIStyle = null;

        private static GUIStyle propertyBoxGUIStyle = null;

        private static GUIStyle preferencesPaneGUIStyle = null;

        private static GUIStyle plainButtonGUIStyle = null;

        private static GUIStyle transparentButtonGUIStyle = null;

        private static GUIStyle welcomeScreenIntroGUIStyle = null;

        private static GUIStyle welcomeScreenTextHeaderGUIStyle = null;

        private static GUIStyle welcomeScreenTextDescriptionGUIStyle = null;

        private static Texture2D taskBorderTexture = null;

        private static Texture2D taskBorderRunningTexture = null;

        private static Texture2D taskBorderIdentifyTexture = null;

        private static Texture2D taskConnectionTexture = null;

        private static Texture2D taskConnectionTopTexture = null;

        private static Texture2D taskConnectionBottomTexture = null;

        private static Texture2D taskConnectionRunningTopTexture = null;

        private static Texture2D taskConnectionRunningBottomTexture = null;

        private static Texture2D taskConnectionIdentifyTopTexture = null;

        private static Texture2D taskConnectionIdentifyBottomTexture = null;

        private static Texture2D taskConnectionCollapsedTexture = null;

        private static Texture2D contentSeparatorTexture = null;

        private static Texture2D docTexture = null;

        private static Texture2D gearTexture = null;

        private static Texture2D syncedTexture = null;

        private static Texture2D sharedTexture = null;

        private static Texture2D variableButtonTexture = null;

        private static Texture2D variableButtonSelectedTexture = null;

        private static Texture2D variableWatchButtonTexture = null;

        private static Texture2D variableWatchButtonSelectedTexture = null;

        private static Texture2D referencedTexture = null;

        private static Texture2D deleteButtonTexture = null;

        private static Texture2D identifyButtonTexture = null;

        private static Texture2D breakpointTexture = null;

        private static Texture2D enableTaskTexture = null;

        private static Texture2D disableTaskTexture = null;

        private static Texture2D expandTaskTexture = null;

        private static Texture2D collapseTaskTexture = null;

        private static Texture2D executionSuccessTexture = null;

        private static Texture2D executionFailureTexture = null;
        
        private static Texture2D prioritySelectorIcon = null;

        private static Texture2D sequenceIcon = null;

        private static Texture2D parallelSelectorIcon = null;

        private static Texture2D inverterIcon = null;

        public static GUIStyle GraphStatusGUIStyle
        {
            get
            {
                if (graphStatusGUIStyle == null)
                {
                    initGraphStatusGUIStyle();
                }
                return graphStatusGUIStyle;
            }
        }

        public static GUIStyle TaskFoldoutGUIStyle
        {
            get
            {
                if (taskFoldoutGUIStyle == null)
                {
                    initTaskFoldoutGUIStyle();
                }
                return taskFoldoutGUIStyle;
            }
        }

        public static GUIStyle TaskTitleGUIStyle
        {
            get
            {
                if (taskTitleGUIStyle == null)
                {
                    initTaskTitleGUIStyle();
                }
                return taskTitleGUIStyle;
            }
        }

        public static GUIStyle TaskGUIStyle
        {
            get
            {
                if (taskGUIStyle == null)
                {
                    initTaskGUIStyle();
                }
                return taskGUIStyle;
            }
        }

        public static GUIStyle TaskSelectedGUIStyle
        {
            get
            {
                if (taskSelectedGUIStyle == null)
                {
                    initTaskSelectedGUIStyle();
                }
                return taskSelectedGUIStyle;
            }
        }

        public static GUIStyle TaskRunningGUIStyle
        {
            get
            {
                if (taskRunningGUIStyle == null)
                {
                    initTaskRunningGUIStyle();
                }
                return taskRunningGUIStyle;
            }
        }

        public static GUIStyle TaskRunningSelectedGUIStyle
        {
            get
            {
                if (taskRunningSelectedGUIStyle == null)
                {
                    initTaskRunningSelectedGUIStyle();
                }
                return taskRunningSelectedGUIStyle;
            }
        }

        public static GUIStyle TaskIdentifyGUIStyle
        {
            get
            {
                if (taskIdentifyGUIStyle == null)
                {
                    initTaskIdentifyGUIStyle();
                }
                return taskIdentifyGUIStyle;
            }
        }

        public static GUIStyle TaskIdentifySelectedGUIStyle
        {
            get
            {
                if (taskIdentifySelectedGUIStyle == null)
                {
                    initTaskIdentifySelectedGUIStyle();
                }
                return taskIdentifySelectedGUIStyle;
            }
        }

        public static GUIStyle TaskCommentGUIStyle
        {
            get
            {
                if (taskCommentGUIStyle == null)
                {
                    initTaskCommentGUIStyle();
                }
                return taskCommentGUIStyle;
            }
        }

        public static GUIStyle TaskCommentLeftAlignGUIStyle
        {
            get
            {
                if (taskCommentLeftAlignGUIStyle == null)
                {
                    initTaskCommentLeftAlignGUIStyle();
                }
                return taskCommentLeftAlignGUIStyle;
            }
        }

        public static GUIStyle TaskCommentRightAlignGUIStyle
        {
            get
            {
                if (taskCommentRightAlignGUIStyle == null)
                {
                    initTaskCommentRightAlignGUIStyle();
                }
                return taskCommentRightAlignGUIStyle;
            }
        }

        public static GUIStyle GraphBackgroundGUIStyle
        {
            get
            {
                if (graphBackgroundGUIStyle == null)
                {
                    initGraphBackgroundGUIStyle();
                }
                return graphBackgroundGUIStyle;
            }
        }

        public static GUIStyle SelectionGUIStyle
        {
            get
            {
                if (selectionGUIStyle == null)
                {
                    initSelectionGUIStyle();
                }
                return selectionGUIStyle;
            }
        }

        public static GUIStyle LabelWrapGUIStyle
        {
            get
            {
                if (labelWrapGUIStyle == null)
                {
                    initLabelWrapGUIStyle();
                }
                return labelWrapGUIStyle;
            }
        }

        public static GUIStyle TaskInspectorCommentGUIStyle
        {
            get
            {
                if (taskInspectorCommentGUIStyle == null)
                {
                    initTaskInspectorCommentGUIStyle();
                }
                return taskInspectorCommentGUIStyle;
            }
        }

        public static GUIStyle TaskInspectorGUIStyle
        {
            get
            {
                if (taskInspectorGUIStyle == null)
                {
                    initTaskInspectorGUIStyle();
                }
                return taskInspectorGUIStyle;
            }
        }

        public static GUIStyle ToolbarButtonSelectionGUIStyle
        {
            get
            {
                if (toolbarButtonSelectionGUIStyle == null)
                {
                    initToolbarButtonSelectionGUIStyle();
                }
                return toolbarButtonSelectionGUIStyle;
            }
        }

        public static GUIStyle PropertyBoxGUIStyle
        {
            get
            {
                if (propertyBoxGUIStyle == null)
                {
                    initPropertyBoxGUIStyle();
                }
                return propertyBoxGUIStyle;
            }
        }

        public static GUIStyle PreferencesPaneGUIStyle
        {
            get
            {
                if (preferencesPaneGUIStyle == null)
                {
                    initPreferencesPaneGUIStyle();
                }
                return preferencesPaneGUIStyle;
            }
        }

        public static GUIStyle PlainButtonGUIStyle
        {
            get
            {
                if (plainButtonGUIStyle == null)
                {
                    initPlainButtonGUIStyle();
                }
                return plainButtonGUIStyle;
            }
        }

        public static GUIStyle TransparentButtonGUIStyle
        {
            get
            {
                if (transparentButtonGUIStyle == null)
                {
                    initTransparentButtonGUIStyle();
                }
                return transparentButtonGUIStyle;
            }
        }

        public static GUIStyle WelcomeScreenIntroGUIStyle
        {
            get
            {
                if (welcomeScreenIntroGUIStyle == null)
                {
                    initWelcomeScreenIntroGUIStyle();
                }
                return welcomeScreenIntroGUIStyle;
            }
        }

        public static GUIStyle WelcomeScreenTextHeaderGUIStyle
        {
            get
            {
                if (welcomeScreenTextHeaderGUIStyle == null)
                {
                    initWelcomeScreenTextHeaderGUIStyle();
                }
                return welcomeScreenTextHeaderGUIStyle;
            }
        }

        public static GUIStyle WelcomeScreenTextDescriptionGUIStyle
        {
            get
            {
                if (welcomeScreenTextDescriptionGUIStyle == null)
                {
                    initWelcomeScreenTextDescriptionGUIStyle();
                }
                return welcomeScreenTextDescriptionGUIStyle;
            }
        }

        public static Texture2D TaskBorderTexture
        {
            get
            {
                if (taskBorderTexture == null)
                {
                    taskBorderTexture = LoadTexture("TaskBorder.png", true, null);
                }
                return taskBorderTexture;
            }
        }

        public static Texture2D TaskBorderRunningTexture
        {
            get
            {
                if (taskBorderRunningTexture == null)
                {
                    taskBorderRunningTexture = LoadTexture("TaskBorderRunning.png", true, null);
                }
                return taskBorderRunningTexture;
            }
        }

        public static Texture2D TaskBorderIdentifyTexture
        {
            get
            {
                if (taskBorderIdentifyTexture == null)
                {
                    taskBorderIdentifyTexture = LoadTexture("TaskBorderIdentify.png", true, null);
                }
                return taskBorderIdentifyTexture;
            }
        }

        public static Texture2D TaskConnectionTexture
        {
            get
            {
                if (taskConnectionTexture == null)
                {
                    taskConnectionTexture = LoadTexture("TaskConnection.png", false, null);
                }
                return taskConnectionTexture;
            }
        }

        public static Texture2D TaskConnectionTopTexture
        {
            get
            {
                if (taskConnectionTopTexture == null)
                {
                    taskConnectionTopTexture = LoadTexture("TaskConnectionTop.png", true, null);           
                }
                return taskConnectionTopTexture;
            }
        }

        public static Texture2D TaskConnectionBottomTexture
        {
            get
            {
                if (taskConnectionBottomTexture == null)
                {
                    taskConnectionBottomTexture = LoadTexture("TaskConnectionBottom.png", true, null);
                }
                return taskConnectionBottomTexture;
            }
        }

        public static Texture2D TaskConnectionRunningTopTexture
        {
            get
            {
                if (taskConnectionRunningTopTexture == null)
                {
                    taskConnectionRunningTopTexture = LoadTexture("TaskConnectionRunningTop.png", true, null);
                }
                return taskConnectionRunningTopTexture;
            }
        }

        public static Texture2D TaskConnectionRunningBottomTexture
        {
            get
            {
                if (taskConnectionRunningBottomTexture == null)
                {
                    taskConnectionRunningBottomTexture = LoadTexture("TaskConnectionRunningBottom.png", true, null);
                }
                return taskConnectionRunningBottomTexture;
            }
        }

        public static Texture2D TaskConnectionIdentifyTopTexture
        {
            get
            {
                if (taskConnectionIdentifyTopTexture == null)
                {
                    taskConnectionIdentifyTopTexture = LoadTexture("TaskConnectionIdentifyTop.png", true, null);
                }
                return taskConnectionIdentifyTopTexture;
            }
        }

        public static Texture2D TaskConnectionIdentifyBottomTexture
        {
            get
            {
                if (taskConnectionIdentifyBottomTexture == null)
                {
                    taskConnectionIdentifyBottomTexture = LoadTexture("TaskConnectionIdentifyBottom.png", true, null);
                }
                return taskConnectionIdentifyBottomTexture;
            }
        }

        public static Texture2D TaskConnectionCollapsedTexture
        {
            get
            {
                if (taskConnectionCollapsedTexture == null)
                {
                    taskConnectionCollapsedTexture = LoadTexture("TaskConnectionCollapsed.png", true, null);
                }
                return taskConnectionCollapsedTexture;
            }
        }

        public static Texture2D ContentSeparatorTexture
        {
            get
            {
                if (contentSeparatorTexture == null)
                {
                    contentSeparatorTexture = LoadTexture("ContentSeparator.png", true, null);
                }
                return contentSeparatorTexture;
            }
        }

        public static Texture2D DocTexture
        {
            get
            {
                if (docTexture == null)
                {
                    docTexture = LoadTexture("DocIcon.png", true, null);
                }
                return docTexture;
            }
        }

        public static Texture2D GearTexture
        {
            get
            {
                if (gearTexture == null)
                {
                    gearTexture = LoadTexture("GearIcon.png", true, null);
                }
                return gearTexture;
            }
        }

        public static Texture2D SyncedTexture
        {
            get
            {
                if (syncedTexture == null)
                {
                    syncedTexture = LoadTexture("SyncedIcon.png", true, null);
                }
                return syncedTexture;
            }
        }

        public static Texture2D SharedTexture
        {
            get
            {
                if (sharedTexture == null)
                {
                    sharedTexture = LoadTexture("SharedIcon.png", true, null);
                }
                return sharedTexture;
            }
        }

        public static Texture2D VariableButtonTexture
        {
            get
            {
                if (variableButtonTexture == null)
                {
                    variableButtonTexture = LoadTexture("VariableButton.png", true, null);
                }
                return variableButtonTexture;
            }
        }

        public static Texture2D VariableButtonSelectedTexture
        {
            get
            {
                if (variableButtonSelectedTexture == null)
                {
                    variableButtonSelectedTexture = LoadTexture("VariableButtonSelected.png", true, null);
                }
                return variableButtonSelectedTexture;
            }
        }

        public static Texture2D VariableWatchButtonTexture
        {
            get
            {
                if (variableWatchButtonTexture == null)
                {
                    variableWatchButtonTexture = LoadTexture("VariableWatchButton.png", true, null);
                }
                return variableWatchButtonTexture;
            }
        }

        public static Texture2D VariableWatchButtonSelectedTexture
        {
            get
            {
                if (variableWatchButtonSelectedTexture == null)
                {
                    variableWatchButtonSelectedTexture = LoadTexture("VariableWatchButtonSelected.png", true, null);
                }
                return variableWatchButtonSelectedTexture;
            }
        }

        public static Texture2D ReferencedTexture
        {
            get
            {
                if (referencedTexture == null)
                {
                    referencedTexture = LoadTexture("LinkedIcon.png", true, null);
                }
                return referencedTexture;
            }
        }

        public static Texture2D DeleteButtonTexture
        {
            get
            {
                if (deleteButtonTexture == null)
                {
                    deleteButtonTexture = LoadTexture("DeleteButton.png", true, null);
                }
                return deleteButtonTexture;
            }
        }

        public static Texture2D IdentifyButtonTexture
        {
            get
            {
                if (identifyButtonTexture == null)
                {
                    identifyButtonTexture = LoadTexture("IdentifyButton.png", true, null);
                }
                return identifyButtonTexture;
            }
        }

        public static Texture2D BreakpointTexture
        {
            get
            {
                if (breakpointTexture == null)
                {
                    breakpointTexture = LoadTexture("BreakpointIcon.png", false, null);
                }
                return breakpointTexture;
            }
        }

        public static Texture2D EnableTaskTexture
        {
            get
            {
                if (enableTaskTexture == null)
                {
                    enableTaskTexture = LoadTexture("TaskEnableIcon.png", false, null);
                }
                return enableTaskTexture;
            }
        }

        public static Texture2D DisableTaskTexture
        {
            get
            {
                if (disableTaskTexture == null)
                {
                    disableTaskTexture = LoadTexture("TaskDisableIcon.png", false, null);
                }
                return disableTaskTexture;
            }
        }

        public static Texture2D ExpandTaskTexture
        {
            get
            {
                if (expandTaskTexture == null)
                {
                    expandTaskTexture = LoadTexture("TaskExpandIcon.png", false, null);
                }
                return expandTaskTexture;
            }
        }

        public static Texture2D CollapseTaskTexture
        {
            get
            {
                if (collapseTaskTexture == null)
                {
                    collapseTaskTexture = LoadTexture("TaskCollapseIcon.png", false, null);
                }
                return collapseTaskTexture;
            }
        }

        public static Texture2D ExecutionSuccessTexture
        {
            get
            {
                if (executionSuccessTexture == null)
                {
                    executionSuccessTexture = LoadTexture("ExecutionSuccess.png", false, null);
                }
                return executionSuccessTexture;
            }
        }

        public static Texture2D ExecutionFailureTexture
        {
            get
            {
                if (executionFailureTexture == null)
                {
                    executionFailureTexture = LoadTexture("ExecutionFailure.png", false, null);
                }
                return executionFailureTexture;
            }
        }
        public static Texture2D PrioritySelectorIcon
        {
            get
            {
                if (prioritySelectorIcon == null)
                {
                    prioritySelectorIcon = LoadTexture("PrioritySelectorIcon.png", false, null);
                }
                return prioritySelectorIcon;
            }
        }

        public static Texture2D SequenceIcon
        {
            get
            {
                if (sequenceIcon == null)
                {
                    sequenceIcon = LoadTexture("SequenceIcon.png", false, null);
                }
                return sequenceIcon;
            }
        }

        public static Texture2D ParallelSelectorIcon
        {
            get
            {
                if (parallelSelectorIcon == null)
                {
                    parallelSelectorIcon = LoadTexture("ParallelSelectorIcon.png", false, null);
                }
                return parallelSelectorIcon;
            }
        }

        public static Texture2D InverterIcon
        {
            get
            {
                if (inverterIcon == null)
                {
                    inverterIcon = LoadTexture("InverterIcon.png", false, null);
                }
                return inverterIcon;
            }
        }

        public static string SplitCamelCase(string s)
        {
            if (s.Equals(""))
            {
                return s;
            }
            if (s.Length > 2 && s.Substring(0, 2).CompareTo("m_") == 0)
            {
                s = s.Substring(2, s.Length - 2);
            }
            Regex regex = new Regex("(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            s = regex.Replace(s, " ");
            s = s.Replace("_", " ");
            return (char.ToUpper(s[0]) + s.Substring(1)).Trim();
        }


        private static string GetEditorBaseDirectory(ScriptableObject obj = null)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string text = Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
            return Path.GetDirectoryName(text.Substring(Application.dataPath.Length - 6));
        }

        public static Texture2D LoadTexture(string imageName, bool useSkinColor = true, ScriptableObject obj = null)
        {
            Texture2D texture2D = null;
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("BehaviorDesignerEditor.Resources.{0}{1}", useSkinColor ? (EditorGUIUtility.isProSkin ? "Dark" : "Light") : "", imageName));
            if (manifestResourceStream != null)
            {
                texture2D = new Texture2D(0, 0);
                texture2D.LoadImage(ReadToEnd(manifestResourceStream));
                manifestResourceStream.Close();
            }
            if (texture2D == null)
            {
                string path = editorPath+"Res/{SkinColor}";
                texture2D = (AssetDatabase.LoadAssetAtPath(path.Replace("{SkinColor}", EditorGUIUtility.isProSkin ? "Dark" : "Light") + imageName, typeof(Texture2D)) as Texture2D);
            }
            if (texture2D != null)
            {
                texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
            }
            return texture2D;
        }

        public static Texture2D LoadIcon(string iconName, ScriptableObject obj = null)
        {
            Texture2D texture2D = null;
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("BehaviorDesignerEditor.Resources.{0}", iconName.Replace("{SkinColor}", EditorGUIUtility.isProSkin ? "Dark" : "Light")));
            if (manifestResourceStream != null)
            {
                texture2D = new Texture2D(0, 0);
                texture2D.LoadImage(ReadToEnd(manifestResourceStream));
                manifestResourceStream.Close();
            }
            if (texture2D == null)
            {

                texture2D = (AssetDatabase.LoadAssetAtPath(editorPath + "Res/" + iconName, typeof(Texture2D)) as Texture2D);
            }
            if (texture2D != null)
            {
                texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
            }
            return texture2D;
        }

        private static byte[] ReadToEnd(Stream stream)
        {
            byte[] array = new byte[16384];
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int count;
                while ((count = stream.Read(array, 0, array.Length)) > 0)
                {
                    memoryStream.Write(array, 0, count);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static void DrawContentSeperator(int yOffset)
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.x = -5f;
            lastRect.y = (lastRect.y + (lastRect.height + yOffset));
            lastRect.height = (2f);
            lastRect.width = (lastRect.width + 10f);
            GUI.DrawTexture(lastRect, ContentSeparatorTexture);
        }

        private static void initGraphStatusGUIStyle()
        {
            graphStatusGUIStyle = new GUIStyle(GUI.skin.label);
            graphStatusGUIStyle.alignment = TextAnchor.MiddleLeft;
            graphStatusGUIStyle.fontSize = 20;
            graphStatusGUIStyle.fontStyle = FontStyle.Bold;
            if (EditorGUIUtility.isProSkin)
            {
                graphStatusGUIStyle.normal.textColor = new Color(0.7058f, 0.7058f, 0.7058f);
                return;
            }
            graphStatusGUIStyle.normal.textColor = new Color(0.8058f, 0.8058f, 0.8058f);
        }

        private static void initTaskFoldoutGUIStyle()
        {
            taskFoldoutGUIStyle = new GUIStyle(EditorStyles.foldout);
            taskFoldoutGUIStyle.alignment = TextAnchor.MiddleLeft;
            taskFoldoutGUIStyle.fontSize = 15;
            taskFoldoutGUIStyle.fontStyle = FontStyle.Bold;
        }

        private static void initTaskTitleGUIStyle()
        {
            taskTitleGUIStyle = new GUIStyle(GUI.skin.label);
            taskTitleGUIStyle.alignment = TextAnchor.UpperCenter;
            taskTitleGUIStyle.fontSize = 12;
            taskTitleGUIStyle.fontStyle = FontStyle.Normal;
        }

        private static void initTaskGUIStyle()
        {
            taskGUIStyle = initTaskGUIStyle(LoadTexture("Task.png", true, null), new RectOffset(5, 3, 3, 5));
        }

        private static void initTaskSelectedGUIStyle()
        {
            taskSelectedGUIStyle = initTaskGUIStyle(LoadTexture("TaskSelected.png", true, null), new RectOffset(5, 4, 4, 4));
        }

        private static void initTaskRunningGUIStyle()
        {
            taskRunningGUIStyle = initTaskGUIStyle(LoadTexture("TaskRunning.png", true, null), new RectOffset(5, 3, 3, 5));
        }

        private static void initTaskRunningSelectedGUIStyle()
        {
            taskRunningSelectedGUIStyle = initTaskGUIStyle(LoadTexture("TaskRunningSelected.png", true, null), new RectOffset(5, 4, 4, 4));
        }

        private static void initTaskIdentifyGUIStyle()
        {
            taskIdentifyGUIStyle = initTaskGUIStyle(LoadTexture("TaskIdentify.png", true, null), new RectOffset(5, 3, 3, 5));
        }

        private static void initTaskIdentifySelectedGUIStyle()
        {
            taskIdentifySelectedGUIStyle = initTaskGUIStyle(LoadTexture("TaskIdentifySelected.png", true, null), new RectOffset(5, 4, 4, 4));
        }

        private static GUIStyle initTaskGUIStyle(Texture2D texture, RectOffset overflow)
        {
            GUIStyle gUIStyle = new GUIStyle(GUI.skin.box);
            gUIStyle.border = (new RectOffset(10, 10, 10, 10));
            gUIStyle.overflow = (overflow);
            gUIStyle.normal.background = (texture);
            gUIStyle.active.background = (texture);
            gUIStyle.hover.background = (texture);
            gUIStyle.focused.background = (texture);
            gUIStyle.normal.textColor = (Color.white);
            gUIStyle.active.textColor = (Color.white);
            gUIStyle.hover.textColor = (Color.white);
            gUIStyle.focused.textColor = (Color.white);
            gUIStyle.stretchHeight = (true);
            gUIStyle.stretchWidth = (true);
            return gUIStyle;
        }

        private static void initTaskCommentGUIStyle()
        {
            taskCommentGUIStyle = new GUIStyle(GUI.skin.label);
            taskCommentGUIStyle.alignment = TextAnchor.UpperCenter;
            taskCommentGUIStyle.fontSize = (12);
            taskCommentGUIStyle.fontStyle = FontStyle.Normal;
            taskCommentGUIStyle.wordWrap = (true);
        }

        private static void initTaskCommentLeftAlignGUIStyle()
        {
            taskCommentLeftAlignGUIStyle = new GUIStyle(GUI.skin.label);
            taskCommentLeftAlignGUIStyle.alignment = TextAnchor.UpperLeft;
            taskCommentLeftAlignGUIStyle.fontSize = (12);
            taskCommentLeftAlignGUIStyle.fontStyle = FontStyle.Normal;
            taskCommentLeftAlignGUIStyle.wordWrap = (false);
        }

        private static void initTaskCommentRightAlignGUIStyle()
        {
            taskCommentRightAlignGUIStyle = new GUIStyle(GUI.skin.label);
            taskCommentRightAlignGUIStyle.alignment = TextAnchor.UpperRight;
            taskCommentRightAlignGUIStyle.fontSize = (12);
            taskCommentRightAlignGUIStyle.fontStyle = FontStyle.Normal;
            taskCommentRightAlignGUIStyle.wordWrap = (false);
        }

        private static void initGraphBackgroundGUIStyle()
        {
            Texture2D texture2D = new Texture2D(1, 1);
            if (EditorGUIUtility.isProSkin)
            {
                texture2D.SetPixel(1, 1, new Color(0.1333f, 0.1333f, 0.1333f));
            }
            else
            {
                texture2D.SetPixel(1, 1, new Color(0.3647f, 0.3647f, 0.3647f));
            }
            texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            texture2D.Apply();
            graphBackgroundGUIStyle = new GUIStyle(GUI.skin.box);
            graphBackgroundGUIStyle.normal.background = (texture2D);
            graphBackgroundGUIStyle.active.background = (texture2D);
            graphBackgroundGUIStyle.hover.background = (texture2D);
            graphBackgroundGUIStyle.focused.background = (texture2D);
            graphBackgroundGUIStyle.normal.textColor = (Color.white);
            graphBackgroundGUIStyle.active.textColor = (Color.white);
            graphBackgroundGUIStyle.hover.textColor = (Color.white);
            graphBackgroundGUIStyle.focused.textColor = (Color.white);
        }

        private static void initSelectionGUIStyle()
        {
            Texture2D texture2D = new Texture2D(1, 1);
            Color color;
            if (EditorGUIUtility.isProSkin)
            {
                color = new Color(0.188f, 0.4588f, 0.6862f, 0.5f);
            }
            else
            {
                color = new Color(0.243f, 0.5686f, 0.839f, 0.5f);
            }
            texture2D.SetPixel(1, 1, color);
            texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            texture2D.Apply();
            selectionGUIStyle = new GUIStyle(GUI.skin.box);
            selectionGUIStyle.normal.background = (texture2D);
            selectionGUIStyle.active.background = (texture2D);
            selectionGUIStyle.hover.background = (texture2D);
            selectionGUIStyle.focused.background = (texture2D);
            selectionGUIStyle.normal.textColor = (Color.white);
            selectionGUIStyle.active.textColor = (Color.white);
            selectionGUIStyle.hover.textColor = (Color.white);
            selectionGUIStyle.focused.textColor = (Color.white);
        }

        private static void initLabelWrapGUIStyle()
        {
            labelWrapGUIStyle = new GUIStyle(GUI.skin.label);
            labelWrapGUIStyle.wordWrap = (true);
            labelWrapGUIStyle.alignment = TextAnchor.MiddleCenter;
        }

        private static void initTaskInspectorCommentGUIStyle()
        {
            taskInspectorCommentGUIStyle = new GUIStyle(GUI.skin.textArea);
            taskInspectorCommentGUIStyle.wordWrap = (true);
        }

        private static void initTaskInspectorGUIStyle()
        {
            taskInspectorGUIStyle = new GUIStyle(GUI.skin.label);
            taskInspectorGUIStyle.alignment = TextAnchor.MiddleLeft;
            taskInspectorGUIStyle.fontSize = (11);
            taskInspectorGUIStyle.fontStyle = FontStyle.Normal;
        }

        private static void initToolbarButtonSelectionGUIStyle()
        {
            toolbarButtonSelectionGUIStyle = new GUIStyle(EditorStyles.toolbarButton);
            toolbarButtonSelectionGUIStyle.normal.background = (toolbarButtonSelectionGUIStyle.active.background);
        }

        private static void initPreferencesPaneGUIStyle()
        {
            preferencesPaneGUIStyle = new GUIStyle(GUI.skin.box);
            preferencesPaneGUIStyle.normal.background = (EditorStyles.toolbarButton.normal.background);
        }

        private static void initPropertyBoxGUIStyle()
        {
            propertyBoxGUIStyle = new GUIStyle();
            propertyBoxGUIStyle.padding = (new RectOffset(2, 2, 0, 0));
        }

        private static void initPlainButtonGUIStyle()
        {
            plainButtonGUIStyle = new GUIStyle(GUI.skin.button);
            plainButtonGUIStyle.border = (new RectOffset(0, 0, 0, 0));
            plainButtonGUIStyle.margin = (new RectOffset(0, 0, 2, 2));
            plainButtonGUIStyle.padding = (new RectOffset(0, 0, 1, 0));
            plainButtonGUIStyle.normal.background = (null);
            plainButtonGUIStyle.active.background = (null);
            plainButtonGUIStyle.hover.background = (null);
            plainButtonGUIStyle.focused.background = (null);
            plainButtonGUIStyle.normal.textColor = (Color.white);
            plainButtonGUIStyle.active.textColor = (Color.white);
            plainButtonGUIStyle.hover.textColor = (Color.white);
            plainButtonGUIStyle.focused.textColor = (Color.white);
        }

        private static void initTransparentButtonGUIStyle()
        {
            transparentButtonGUIStyle = new GUIStyle(GUI.skin.button);
            transparentButtonGUIStyle.border = (new RectOffset(0, 0, 0, 0));
            transparentButtonGUIStyle.margin = (new RectOffset(4, 4, 2, 2));
            transparentButtonGUIStyle.padding = (new RectOffset(2, 2, 1, 0));
            transparentButtonGUIStyle.normal.background = (null);
            transparentButtonGUIStyle.active.background = (null);
            transparentButtonGUIStyle.hover.background = (null);
            transparentButtonGUIStyle.focused.background = (null);
            transparentButtonGUIStyle.normal.textColor = (Color.white);
            transparentButtonGUIStyle.active.textColor = (Color.white);
            transparentButtonGUIStyle.hover.textColor = (Color.white);
            transparentButtonGUIStyle.focused.textColor = (Color.white);
        }

        private static void initWelcomeScreenIntroGUIStyle()
        {
            welcomeScreenIntroGUIStyle = new GUIStyle(GUI.skin.label);
            welcomeScreenIntroGUIStyle.fontSize = (16);
            welcomeScreenIntroGUIStyle.fontStyle = FontStyle.Bold;
            welcomeScreenIntroGUIStyle.normal.textColor = (new Color(0.706f, 0.706f, 0.706f));
        }

        private static void initWelcomeScreenTextHeaderGUIStyle()
        {
            welcomeScreenTextHeaderGUIStyle = new GUIStyle(GUI.skin.label);
            welcomeScreenTextHeaderGUIStyle.alignment = TextAnchor.MiddleLeft;
            welcomeScreenTextHeaderGUIStyle.fontSize = (14);
            welcomeScreenTextHeaderGUIStyle.fontStyle = FontStyle.Bold;
        }

        private static void initWelcomeScreenTextDescriptionGUIStyle()
        {
            welcomeScreenTextDescriptionGUIStyle = new GUIStyle(GUI.skin.label);
            welcomeScreenTextDescriptionGUIStyle.wordWrap = (true);
        }
    }
}
