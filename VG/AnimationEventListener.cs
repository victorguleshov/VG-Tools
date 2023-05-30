using System;
using UnityEngine;

namespace VG
{
    public class AnimationEventListener : MonoBehaviour
    {
        public event Action<string> listener;

        public void SendEvent(string eventName)
        {
            listener?.Invoke(eventName);
        }
    }
}