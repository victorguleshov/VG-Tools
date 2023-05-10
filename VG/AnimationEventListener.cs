using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public event Action<string> listener;
    
    public void SendEvent (string eventName)
    {
        listener?.Invoke (eventName);
    }
}