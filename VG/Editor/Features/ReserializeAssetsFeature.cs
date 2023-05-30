#if UNITY_EDITOR

using System.Linq;
using UnityEditor;

namespace VG.Editor.Features
{
    public class ReserializeAssetsFeature
    {
        [MenuItem("Assets/VG/Reserialize All", false, 40)]
        private static void ReserializeAllAssets()
        {
            var pathes = AssetDatabase.GetAllAssetPaths().ToList();
            AssetDatabase.ForceReserializeAssets(pathes);
        }

        [MenuItem("Assets/VG/Reserialize Selected", false, 40)]
        public static void ReserializeSelectedAssets()
        {
            var pathes = Selection.objects.Select(AssetDatabase.GetAssetPath).ToList();
            AssetDatabase.ForceReserializeAssets(pathes);
        }

        [MenuItem("Assets/VG/Reserialize Selected", true)]
        private static bool ValidateReserializeSelectedAssets() =>
            EditorUtility.IsPersistent(Selection.activeGameObject);
    }
}

#endif