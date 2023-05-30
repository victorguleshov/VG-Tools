#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.U2D;

namespace VG.Editor.Features
{
    public static class SpriteAtlasSetPaddingFeature
    {
        [MenuItem("Assets/VG/SpriteAtlas Set Padding/0")]
        public static void SpriteAtlasCustomPadding0()
        {
            SpriteAtlasCustomPadding(0);
        }

        [MenuItem("Assets/VG/SpriteAtlas Set Padding/16")]
        public static void SpriteAtlasCustomPadding16()
        {
            SpriteAtlasCustomPadding(16);
        }

        [MenuItem("Assets/VG/SpriteAtlas Set Padding/32")]
        public static void SpriteAtlasCustomPadding32()
        {
            SpriteAtlasCustomPadding(32);
        }

        private static void SpriteAtlasCustomPadding(int value)
        {
            var objects = Selection.objects;

            foreach (var item in objects)
            {
                var atlas = item as SpriteAtlas;
                if (atlas)
                {
                    var settings = atlas.GetPackingSettings();
                    settings.padding = value;
                    atlas.SetPackingSettings(settings);
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}

#endif