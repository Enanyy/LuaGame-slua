
using UnityEngine;

namespace BTree.Editor
{
    public class BTEditorZoomArea
    {
        private static Matrix4x4 mPrevGuiMatrix;

        public static Rect Begin(Rect screenCoordsArea, float zoomScale)
        {
            GUI.EndGroup();
            Rect rect = screenCoordsArea;
            //Rect rect = screenCoordsArea.ScaleSizeBy(1f / zoomScale, screenCoordsArea.TopLeft());
            rect.y = (rect.y + BTEditorUtility.EditorWindowTabHeight);
            GUI.BeginGroup(rect);
            mPrevGuiMatrix = GUI.matrix;
            Matrix4x4 matrix4x = Matrix4x4.TRS(rect.TopLeft(), Quaternion.identity, Vector3.one);
            Matrix4x4 matrix4x2 = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1f));
            GUI.matrix = (matrix4x * matrix4x2 * matrix4x.inverse * GUI.matrix);
            return rect;
        }

        public static void End()
        {
            GUI.matrix = (mPrevGuiMatrix);
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0f, BTEditorUtility.EditorWindowTabHeight, Screen.width, Screen.height));
        }
    }
}
