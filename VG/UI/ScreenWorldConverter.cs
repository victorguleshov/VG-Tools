using UnityEngine;

namespace VG.UI
{
    public static class ScreenSizeConverter
    {
        public static float GetScreenToWorldHeight
        {
            get
            {
                var topRightCorner = new Vector2(1, 1);
                Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
                var height = edgeVector.y * 2;
                return height;
            }
        }

        public static float GetScreenToWorldWidth
        {
            get
            {
                var topRightCorner = new Vector2(1, 1);
                Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
                var width = edgeVector.x * 2;
                return width;
            }
        }
    }
}