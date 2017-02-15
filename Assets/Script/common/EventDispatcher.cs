using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public class EventDispatcher
    {
        private Dictionary<string, List<IEvent>> listeners;

        private static EventDispatcher _instance;
        public static EventDispatcher instance
        {
            get
            {
                if (EventDispatcher._instance == null)
                    EventDispatcher._instance = new EventDispatcher();
                return EventDispatcher._instance;
            }
        }

        public EventDispatcher()
        {
            listeners = new Dictionary<string, List<IEvent>>();
        }

        public void addEventListener(string eventType, IEvent item)
        {
            if (listeners.ContainsKey(eventType))
            {
                if (listeners[eventType].Contains(item))
                    return;
            }
            else
            {
                listeners.Add(eventType, new List<IEvent>());
            }

            listeners[eventType].Add(item);
        }

        public void removeEventListener(string eventType, IEvent item)
        {
            if (!listeners.ContainsKey(eventType))
                return;

            listeners[eventType].Remove(item);
        }

        public void dispatchEvent(string eventType, object param = null)
        {
            if (!listeners.ContainsKey(eventType))
                return;

            List<IEvent> e = listeners[eventType];
            for (var i = e.Count - 1; i > -1; i--)
            {
                e[i].handler(eventType, param);
            }

        }
    }


    public interface IEvent
    {
        void handler(string eventName, Object param);
    }


    public class GameEventType
    {
        public const string TRIGGER_FIGHT = "triggerFight";
        public const string END_FIGHT = "endFight";
        public const string TRIGGER_DRAMA = "triggerDrama";
    }
}
