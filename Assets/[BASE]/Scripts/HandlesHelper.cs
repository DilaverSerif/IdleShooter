using UnityEditor;
using UnityEngine;

namespace _BASE_.Scripts
{
    public static class HandlesHelper
    {
#if UNITY_EDITOR
        public static void DrawSolidArc(Vector3 center, Vector3 forward, Vector3 up, float angle, float radius, Color color = default(Color))   
        {
            Handles.color = color;
            Handles.DrawSolidArc(center, forward, up, angle, radius);
        }
        
        public static void DrawDisc(Vector3 center, float radius, Color color, float thickness = 3f)
        {
            Handles.color = color;
            Handles.DrawWireDisc(center, Vector3.down, radius, thickness);
        }

        public static void DrawText(Vector3 position, string text, Color32 color)
        {
            Handles.Label(position, text, new GUIStyle() {normal = new GUIStyleState() {textColor = color}});
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float thickness = 3f)
        {
            Handles.color = color;
            Handles.DrawLine(start, end, thickness);
        }
        
        public static void DrawArc(Vector2 center, Vector2 startDir, Vector2 endDir, float radius)
        {
            Color c = Handles.color;
            Handles.color = Gizmos.color;

            float angle = Vector2.SignedAngle(startDir.normalized, endDir.normalized);
            if (angle > 0) angle -= 360;
            Handles.DrawWireArc(center, Vector3.forward, startDir, angle, radius);

            Handles.color = c;
        }
        
        public static void DrawCone(Vector3 position, Quaternion rotation, float angle, float radius)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireArc(position, rotation * Vector3.up, rotation * Vector3.forward, angle, radius);
            Handles.DrawWireArc(position, rotation * Vector3.forward, rotation * Vector3.down, angle, radius);
            Handles.DrawWireArc(position, rotation * Vector3.down, rotation * Vector3.back, angle, radius);
            Handles.DrawWireArc(position, rotation * Vector3.back, rotation * Vector3.up, angle, radius);
        }
        
        public static void DrawSolidCircle(Vector3 center, Vector3 forward, float radius,Color color = default(Color))
        {
            Color c = Handles.color;
            Handles.color = color;

            Handles.DrawSolidDisc(
                center, forward, radius);

            Handles.color = c;
        }

#endif
    }
}