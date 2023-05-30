using UnityEngine;

namespace VG.UI
{
    public class ScreenSizeAdjuster : MonoBehaviour
    {
        [SerializeField] private float sizeRate;

        private void Start()
        {
            var width = ScreenSizeConverter.GetScreenToWorldHeight;
            transform.localScale = Vector3.one * width * sizeRate;
        }
    }
}