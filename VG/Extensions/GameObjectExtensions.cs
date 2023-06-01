using UnityEngine;

namespace VG.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsPrefab(this GameObject gameObject) =>
            gameObject.scene == default;
    }
}