using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//通知管理
namespace Notification
{

    public delegate void NotificationDelegate(NotificationMessage msg);
    public class NotificationMessage
    {
        public string message;
        public object objectInfo;
        public NotificationMessage(string message,object objectInfo)
        {
            this.message = message;
            this.objectInfo = objectInfo;
        }
    }

    public class NotificationCenter
    {
        private static NotificationCenter _default = null;
        public static NotificationCenter Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new NotificationCenter();
                }
                return _default;
            }
        }
        
        Dictionary<string, Dictionary<object, NotificationDelegate>> _dic;

        public NotificationCenter()
        {
            _dic = new Dictionary<string, Dictionary<object, NotificationDelegate>>();
        }

        public void Register(string name, object target ,NotificationDelegate action)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (target == null)
            {
                throw new ArgumentNullException("action");
            }
            AddValue(name, target, action);
        }

        void AddValue(string key1,object key2,NotificationDelegate val)
        {
            Dictionary<object, NotificationDelegate> kv = null;
            if (_dic.TryGetValue(key1, out kv))
            {
                if (kv != null)
                {
                    if (kv.ContainsKey(key2))
                    {
                        kv.Remove(key2);
                    }
                    kv[key2] = val;
                }
            }
            else
            {
                kv = new Dictionary<object, NotificationDelegate>();
                kv[key2] = val;
                _dic.Add(key1, kv);
            }
        }

        public void RemoveRegister(string name, object target)
        {
            if (!_dic.ContainsKey(name))
            {
                Debug.Log("移除操作：没有这个通知");
                return;
            }

            Dictionary<object, NotificationDelegate> kv = null;
            if (_dic.TryGetValue(name, out kv))
            {
                if (kv.ContainsKey(target))
                {
                    kv.Remove(target);
                }
                _dic[name] = kv;
            }
        }

        public void Post(string name, object obj)
        {
            if (!_dic.ContainsKey(name))
            {
                Debug.Log("抛通知操作：没有这个通知");
                return;
            }
            var msg = new NotificationMessage(name, obj);
            var dic = _dic[name];
            foreach (var kv in dic.Values)
            {
                kv(msg);
            }
        }
    }
}
