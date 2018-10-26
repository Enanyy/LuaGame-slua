
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public enum NodeConnectionType
    {
        Incoming,
        Outgoing,
        Fixed
    }
    public class BTNodeConnection
    {
        public BTNodeDesigner mDestinationNodeDesigner;
        public BTNodeDesigner mOriginatingNodeDesigner;
        public float HorizontalHeight;

        public NodeConnectionType NodeConnectionType;
        private bool mSelected;
        
        public BTNodeConnection(BTNodeDesigner _dest, BTNodeDesigner _orig, NodeConnectionType _type)
        {
            mDestinationNodeDesigner = _dest;
            mOriginatingNodeDesigner = _orig;
            NodeConnectionType = _type;
        }

        //绘制连线
        public void DrawConnection(Vector2 offset, float graphZoom, bool disabled)
        {
            DrawConnection(mOriginatingNodeDesigner.GetConnectionPosition(offset, NodeConnectionType.Outgoing), mDestinationNodeDesigner.GetConnectionPosition(offset, NodeConnectionType.Incoming), graphZoom, disabled);
        }
        //绘制连线
        public void DrawConnection(Vector2 source, Vector2 destination, float graphZoom, bool disabled)
        {
            Color color = disabled ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
            Handles.color = color;
            Vector3[] array = new Vector3[]
            {
                source,
                new Vector2(source.x, HorizontalHeight),
                new Vector2(destination.x, HorizontalHeight),
                destination
            };
            Handles.DrawAAPolyLine(BTEditorUtility.TaskConnectionTexture, BTEditorUtility.LineWidth / graphZoom, array);
        }
        
    }
}
