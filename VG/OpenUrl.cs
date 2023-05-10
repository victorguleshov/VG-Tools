using UnityEngine;

namespace VG.Utilities
{
    public class OpenUrl : MonoBehaviour
    {
        public void Open(string url)
        {
            Application.OpenURL(url);
        }
    }
}