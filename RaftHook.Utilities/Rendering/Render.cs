using System.Collections.Generic;
using UnityEngine;

namespace RaftHook.Utilities
{
    public static class Render
    {
        private static readonly Dictionary<int, RingArray> RingDict = new Dictionary<int, RingArray>();
        private static readonly GUIStyle Style = new GUIStyle();
        private static readonly GUIStyle OutlineStyle = new GUIStyle();
        public static GUIStyle StringStyle { get; set; } = new GUIStyle(GUI.skin.label);

        public static Color Color
        {
            get => GUI.color;
            set => GUI.color = value;
        }

        public static bool IsOnScreen(Vector3 position)
        {
            return position.y > 0.01f && position.y < Screen.height - 5f && position.z > 0.01f;
        }

        public static void DrawLine(Vector2 from, Vector2 to, float thickness, Color color)
        {
            Color = color;
            DrawLine(from, to, thickness);
        }

        public static void DrawLine(Vector2 from, Vector2 to, float thickness)
        {
            var normalized = (to - from).normalized;
            var num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
            GUIUtility.RotateAroundPivot(num, from);
            DrawBox(from, Vector2.right * (from - to).magnitude, thickness, false);
            GUIUtility.RotateAroundPivot(-num, from);
        }

        public static void DrawBox(Vector2 position, Vector2 size, float thickness, Color color, bool centered = true)
        {
            Color = color;
            DrawBox(position, size, thickness, centered);
        }

        public static void DrawBox(Vector2 position, Vector2 size, float thickness, bool centered = true)
        {
            if (centered) position -= size / 2f;
            GUI.DrawTexture(new Rect(position.x, position.y, size.x, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x, position.y, thickness, size.y), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x + size.x, position.y, thickness, size.y), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x, position.y + size.y, size.x + thickness, thickness),
                Texture2D.whiteTexture);
        }

        public static void DrawCross(Vector2 position, Vector2 size, float thickness, Color color)
        {
            Color = color;
            DrawCross(position, size, thickness);
        }

        public static void DrawCross(Vector2 position, Vector2 size, float thickness)
        {
            GUI.DrawTexture(new Rect(position.x - size.x / 2f, position.y, size.x, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x, position.y - size.y / 2f, thickness, size.y), Texture2D.whiteTexture);
        }

        public static void DrawDot(Vector2 position, Color color)
        {
            Color = color;
            DrawDot(position);
        }

        public static void DrawDot(Vector2 position)
        {
            DrawBox(position - Vector2.one, Vector2.one * 2f, 1f);
        }

        public static void DrawString(Vector2 pos, string text, Color color, bool center = true, int size = 12,
            FontStyle fontStyle = FontStyle.Bold, int depth = 1)
        {
            Style.fontSize = size;
            Style.richText = true;
            Style.normal.textColor = color;
            Style.fontStyle = fontStyle;
            OutlineStyle.fontSize = size;
            OutlineStyle.richText = true;
            OutlineStyle.normal.textColor = new Color(0f, 0f, 0f, 1f);
            OutlineStyle.fontStyle = fontStyle;
            var guicontent = new GUIContent(text);
            var guicontent2 = new GUIContent(text);
            if (center) pos.x -= Style.CalcSize(guicontent).x / 2f;
            switch (depth)
            {
                case 0:
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Style);
                    return;
                case 1:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), guicontent2, OutlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Style);
                    return;
                case 2:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), guicontent2, OutlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), guicontent2, OutlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Style);
                    return;
                case 3:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), guicontent2, OutlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), guicontent2, OutlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y - 1f, 300f, 25f), guicontent2, OutlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y + 1f, 300f, 25f), guicontent2, OutlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Style);
                    return;
                default:
                    return;
            }
        }

        public static void DrawCircle(Vector2 position, float radius, int numSides, bool centered = true,
            float thickness = 1f)
        {
            DrawCircle(position, radius, numSides, Color.white, centered, thickness);
        }

        public static void DrawCircle(Vector2 position, float radius, int numSides, Color color, bool centered = true,
            float thickness = 1f)
        {
            RingArray ringArray;
            if (RingDict.TryGetValue(numSides, out var value))
                ringArray = value;
            else
                ringArray = RingDict[numSides] = new RingArray(numSides);
            var vector = centered ? position : position + Vector2.one * radius;
            for (var i = 0; i < numSides - 1; i++)
                DrawLine(vector + ringArray.Positions[i] * radius, vector + ringArray.Positions[i + 1] * radius,
                    thickness, color);
            DrawLine(vector + ringArray.Positions[0] * radius,
                vector + ringArray.Positions[ringArray.Positions.Length - 1] * radius, thickness, color);
        }

        private class RingArray
        {
            public RingArray(int numSegments)
            {
                Positions = new Vector2[numSegments];
                var num = 360f / numSegments;
                for (var i = 0; i < numSegments; i++)
                {
                    var num2 = 0.017453292f * num * i;
                    Positions[i] = new Vector2(Mathf.Sin(num2), Mathf.Cos(num2));
                }
            }

            public Vector2[] Positions { get; }
        }
    }
}