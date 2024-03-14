using System;
using System.Collections.Generic;
using UnityEngine;

namespace njh
{
    public class EventManager : Singleton<EventManager>
    {
        //调用事件好像可以不用写泛型，好像可以通过params直接传参

        //key —— 事件的名字（比如：怪物死亡，玩家死亡，通关 等等）
        //value —— 对应的是 监听这个事件 对应的委托函数们
        private Dictionary<string, List<Delegate>> eventDic = new Dictionary<string, List<Delegate>>();

        #region AddListener
        public void AddListenerBase(string eventName, Delegate callback)
        {
            if (eventDic.ContainsKey(eventName))
                eventDic[eventName].Add(callback);
            else
                eventDic.Add(eventName, new List<Delegate>() { callback });
        }


        public void AddListener(string eventName, Action callback) => AddListenerBase(eventName, callback);
        public void AddListener<T>(string eventName, Action<T> callback) => AddListenerBase(eventName, callback);

        public void AddListener<T1, T2>(string eventName, Action<T1, T2> callback) => AddListenerBase(eventName, callback);

        public void AddListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> callback) => AddListenerBase(eventName, callback);

        public void AddListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callback) => AddListenerBase(eventName, callback);

        #endregion

        #region RemoveListener
        public void RemoveListenerBase(string eventName, Delegate callback)
        {
            if (eventName == null) return;
            if (eventDic.TryGetValue(eventName, out List<Delegate> eventList))
            {
                eventList.Remove(callback);
                //事件列表没有事件时，将该事件键值对从字典中移除
                if (eventList.Count == 0)
                {
                    eventDic.Remove(eventName);
                }
            }
        }

        public void RemoveAllListener(string eventName)
        {
            if (eventName == null) return;
            if (eventDic.ContainsKey(eventName))
                eventDic.Remove(eventName);
        }

        public void RemoveListener(string eventName, Action callback) => RemoveListenerBase(eventName, callback);

        public void RemoveListener<T>(string eventName, Action<T> callback) => RemoveListenerBase(eventName, callback);

        public void RemoveListener<T1, T2>(string eventName, Action<T1, T2> callback) => RemoveListenerBase(eventName, callback);

        public void RemoveListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> callback) => RemoveListenerBase(eventName, callback);

        public void RemoveListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callback) => RemoveListenerBase(eventName, callback);

        #endregion

        #region TriggerEvent

        public void TriggerEvent(string eventName)
        {
            if (eventDic.ContainsKey(eventName))
            {
                foreach (Delegate callback in eventDic[eventName])
                {
                    (callback as Action)?.Invoke();
                }
            }
        }

        public void TriggerEvent<T>(string eventName, T info)
        {
            if (eventDic.ContainsKey(eventName))
            {
                foreach (Delegate callback in eventDic[eventName])
                {
                    (callback as Action<T>)?.Invoke(info);
                }
            }
        }

        public void TriggerEvent<T1, T2>(string eventName, T1 info1, T2 info2)
        {
            if (eventDic.ContainsKey(eventName))
            {
                foreach (Delegate callback in eventDic[eventName])
                {
                    (callback as Action<T1, T2>)?.Invoke(info1, info2);
                }
            }
        }

        public void TriggerEvent<T1, T2, T3>(string eventName, T1 info1, T2 info2, T3 info3)
        {
            if (eventDic.ContainsKey(eventName))
            {
                foreach (Delegate callback in eventDic[eventName])
                {
                    (callback as Action<T1, T2, T3>)?.Invoke(info1, info2, info3);
                }
            }
        }

        public void TriggerEvent<T1, T2, T3, T4>(string eventName, T1 info1, T2 info2, T3 info3, T4 info4)
        {
            if (eventDic.ContainsKey(eventName))
            {
                foreach (Delegate callback in eventDic[eventName])
                {
                    (callback as Action<T1, T2, T3, T4>)?.Invoke(info1, info2, info3, info4);
                }
            }
        }
        #endregion

        public string GetRandomKey()
        {
            return Calc.GetUnuseStrInDic(eventDic);
        }

        public void Clear()
        {
            eventDic.Clear();
        }

        //新功能拓展测试，现在只设置传Func float
        private Dictionary<string, Func<float>> funcDic = new Dictionary<string, Func<float>>();

        public bool TryTrigerFunc(string name,out float res)
        {
            res = 0;
            if(funcDic.TryGetValue(name,out Func<float> func))
            {
                res = func();
                return true;
            }
            return false;
        }

        public void AddFuncListener(string funcName, Func<float>callback)
        {
            if (funcDic.ContainsKey(funcName))
                funcDic[funcName] = callback;
            else
                funcDic.Add(funcName, callback);
        }

        public void RemoveFuncListener(string funcName)
        {
            if (funcName == null) return;
            funcDic.Remove(funcName);
        }

    }

}
