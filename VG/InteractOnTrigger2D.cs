using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VG.Extensions;

namespace VG
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractOnTrigger2D : MonoBehaviour
    {
        public LayerMask layers = -1;
        public TriggerEvent onTriggerEnter, onTriggerExit;
        public List<Collider2D> overlapped = new();

        private void OnDisable()
        {
            foreach (var other in overlapped) onTriggerExit.Invoke(other);
            overlapped.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (layers.Contains(other.gameObject))
            {
                overlapped.Add(other);
                onTriggerEnter.Invoke(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (layers.Contains(other.gameObject))
            {
                overlapped.Remove(other);
                onTriggerExit.Invoke(other);
            }
        }

        [Serializable]
        public class TriggerEvent : UnityEvent<Collider2D> { }
    }
}