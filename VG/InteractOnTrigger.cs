using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VG.Extensions;

namespace VG
{
    [RequireComponent(typeof(Collider))]
    public class InteractOnTrigger : MonoBehaviour
    {
        public LayerMask layers = -1;
        public TriggerEvent onTriggerEnter, onTriggerExit;
        public List<Collider> overlapped = new();

        private void OnDisable()
        {
            foreach (var other in overlapped) onTriggerExit.Invoke(other);
            overlapped.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (layers.Contains(other.gameObject))
            {
                overlapped.Add(other);
                onTriggerEnter.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (layers.Contains(other.gameObject))
            {
                overlapped.Remove(other);
                onTriggerExit.Invoke(other);
            }
        }

        [Serializable]
        public class TriggerEvent : UnityEvent<Collider> { }
    }
}