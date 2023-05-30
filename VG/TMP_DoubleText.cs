using TMPro;
using UnityEngine;

namespace VG
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshPro))]
    public class TMP_DoubleText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro copyFrom;
        private TextMeshPro to;

        private void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            if (!to) to = GetComponent<TextMeshPro>();
        }

        private void LateUpdate()
        {
            if (!to) Reset();
            if (copyFrom)
                if (copyFrom.text != to.text)
                    to.text = copyFrom.text;
        }
    }
}