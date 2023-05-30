using TMPro;
using UnityEngine;

namespace VG
{
    [RequireComponent(typeof(TMP_Text))]
    public class Version : MonoBehaviour
    {
        public string format = "Version: {0}";

        private void Start()
        {
            var text = GetComponent<TMP_Text>();
            text.text = string.Format(format, Application.version);
        }
    }
}