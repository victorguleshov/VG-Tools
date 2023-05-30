using UnityEngine;
using UnityEngine.Events;

namespace VG
{
    public class OnAwakeTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onAwake;

        private void Awake()
        {
            onAwake?.Invoke();
        }
    }
}