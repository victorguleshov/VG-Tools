using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VG
{
    public class ButtonMirrorOnClick : MonoBehaviour, IPointerClickHandler
    {
        [Header("Invoke listener of this button")] [SerializeField]
        private Button button;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (button) button.onClick.Invoke();
        }
    }
}