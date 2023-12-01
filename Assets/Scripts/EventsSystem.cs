using System;
using System.Collections.Generic;

namespace RPS
{
    public class EventsSystem
    {
        static Dictionary<string, List<Action>> _events = new Dictionary<string, List<Action>>();
        public static void Subscribe(string eventName, Action action)
        {
            if (!_events.ContainsKey(eventName))
            {
                _events.Add(eventName, new List<Action>());
            }
            _events[eventName].Add(action);
        }
        
        public static void Emit(string eventName)
        {
            if (!_events.ContainsKey(eventName))
            {
                return;
            }
            foreach (var action in _events[eventName])
            {
                action();
            }
        }
    }
}