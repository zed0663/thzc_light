using System;
using com.ootii.Messages;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Monster.Events
{
    public class EventSystemManager
    {
        #region AddEvent
        public static void AddEvent(string name, Action obj)
        {
            MessageDispatcher.AddListener(name, new MessageHandler((IMessage rMessage) =>
            {
                obj();
            }));
        }

        public static void AddEvent(UnityEngine.Object Owner, string name, Action obj)
        {
            MessageDispatcher.AddListener(Owner, name, new MessageHandler((IMessage rMessage) =>
            {
                obj();
            }));
        }

        public static void AddEvent<T>(string name, Action<T> obj)
        {
            MessageDispatcher.AddListener(name, new MessageHandler((IMessage rMessage) =>
            {
                obj((T)rMessage.Data);
            }));
        }

        public static void AddEvent<T>(UnityEngine.Object Owner, string name, Action<T> obj)
        {
            MessageDispatcher.AddListener(Owner, name, new MessageHandler((IMessage rMessage) =>
            {
                obj((T)rMessage.Data);
            }));
        }

        public void EventLListener()
        {

        }
        #endregion

        #region AddEvent

        public static void SendMessage(string name,float delay=0)
        {
            MessageDispatcher.SendMessage(name, delay);
        }

        public static void SendMessage<T>(object Owner, string name, T value, float delay = 0)
        {
            MessageDispatcher.SendMessage(Owner, name, value, delay);
        }

        public static void SendMessage<T>(string name, T value, float delay = 0)
        {
            MessageDispatcher.SendMessageData(name, value, delay);
        }
        #endregion

        #region RemoveEvent

        

        #endregion

    }
}
