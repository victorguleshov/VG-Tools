using System.Collections.Generic;
using VG.Extensions;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Collider))]
public class InteractOnTrigger : MonoBehaviour
{
    public LayerMask layers = -1;
    public TriggerEvent onTriggerEnter, onTriggerExit;
    public List<Collider> overlapped = new List<Collider> ();

    private void OnTriggerEnter (Collider other)
    {
        if (layers.Contains (other.gameObject))
        {
            overlapped.Add (other);
            onTriggerEnter.Invoke (other);
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (layers.Contains (other.gameObject))
        {
            overlapped.Remove (other);
            onTriggerExit.Invoke (other);
        }
    }

    private void OnDisable ()
    {
        foreach (var other in overlapped)
        {
            onTriggerExit.Invoke (other);
        }
        overlapped.Clear ();
    }

    [System.Serializable] public class TriggerEvent : UnityEvent<Collider> { }
}