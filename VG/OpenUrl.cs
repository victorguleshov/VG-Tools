using UnityEngine;

namespace VG
{
    public class OpenUrl : MonoBehaviour
    {
        public void Open(string url)
        {
            Application.OpenURL(url);
        }
    }
}