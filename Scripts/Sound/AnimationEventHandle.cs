using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DoDoDoIt.Utility
{
    [Serializable]
    public class StringByEvent
    {
        public string Key;
        public UnityEvent UnityEvent;
    }

    public class AnimationEventHandle : MonoBehaviour
    {
        public StringByEvent[] StringByEvents;
        private readonly Dictionary<string, UnityEvent> _events = new();

        private void Awake()
        {
            foreach (var stringByEvent in StringByEvents)
            {
                if (_events.ContainsKey(stringByEvent.Key))
                {
                    Debug.LogWarning($"Duplicate key {stringByEvent.Key} at {gameObject.name} !!!");
                }

                _events.Add(stringByEvent.Key, stringByEvent.UnityEvent);
            }
        }

        public void OnAnimationEvent(string id)
        {
            if (_events.TryGetValue(id, out var unityEvent))
            {
                unityEvent?.Invoke();
            }
        }
    }
}