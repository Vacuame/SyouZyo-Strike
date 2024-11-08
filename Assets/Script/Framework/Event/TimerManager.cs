using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : Singleton<TimerManager>
{
    public Dictionary<int, Timer> timerDic = new Dictionary<int, Timer>();
    public LinkedList<Timer> timerList = new LinkedList<Timer>();

    public override void Init()
    {
        GameRoot.Instance.beforeLoadSceneAction += RemoveAllTimers;
    }

    public void Update()
    {
        List<int> removes = new List<int>();

        foreach (var a in timerDic)
        {
            Timer t = a.Value;
            t.TimePassBy();
            if (t.isOver)
                removes.Add(t.id);
        }

        foreach (var key in removes)
            RemoveTimer(key);
    }

    public bool TryGetTimer(int key, out Timer t)
    {
        t = null;
        if (timerDic.ContainsKey(key))
        {
            t = timerDic[key];
            return true;
        }
        return false;
    }

    public void RemoveTimer(int key)
    {
        if (timerDic.ContainsKey(key))
        {
            timerDic[key].Clear();
            timerDic.Remove(key);
        }
    }

    public void RemoveTimers(params int[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
            RemoveTimer(keys[i]);
    }

    public void RemoveAllTimers()
    {
        int[] keys = new int[timerDic.Count];
        int index = 0;
        foreach(var a in timerDic)
        {
            keys[index++] = a.Key;
        }
        RemoveTimers(keys);
    }

    public int AddTimer(Timer timer)
    {
        int newId;
        do
        {
            newId = Random.Range(int.MinValue, int.MaxValue);
        }
        while (timerDic.ContainsKey(newId));
        timer.id = newId;
        timerDic.Add(newId, timer);
        return newId;
    }


}

public class Timer
{
    public int id;
    public float timer;
    private int intervalIndex;
    private List<float> intervals;
    public int loopCount { get; private set; }//小于0是无限
    private UnityAction action;
    private string eventName;
    bool haveEvent = false;
    public bool pause;
    private bool unscaled;
    bool updateInvoke;

    public bool isOver { get { return timer <= 0 && loopCount == 0; } }
    #region 构造

    //新增兼容事件系统
    public Timer(string eventKey, bool invokeInUpdate, int loopCount, params float[] intervals)
    {
        this.haveEvent = true;
        this.eventName = eventKey;
        this.updateInvoke = invokeInUpdate;
        this.loopCount = loopCount;
        this.intervalIndex = 0;
        this.intervals = new List<float>();
        for (int i = 0; i < intervals.Length; i++)
            this.intervals.Add(intervals[i]);
    }

    private Timer(UnityAction action, int loopCount)
    {
        this.loopCount = loopCount;
        this.intervalIndex = 0;
        this.action = action;
    }
    public Timer(UnityAction action, int loopCount, List<float> intervals, bool unscaled = false) : this(action, loopCount)
    {
        this.unscaled = unscaled;
        this.intervals = intervals;
        ToNextInterval();
    }
    //一般用这个
    public Timer(UnityAction action, int loopCount, params float[] intervals) : this(action, loopCount)
    {
        this.intervals = new List<float>();
        for (int i = 0; i < intervals.Length; i++)
            this.intervals.Add(intervals[i]);
        ToNextInterval();
    }

    #endregion

    private void ToNextInterval()
    {
        if (loopCount == 0) //防止减成-1（无限）
        {
            timer = 0;
            return;
        }

        timer = intervals[intervalIndex++];
        if (intervalIndex >= intervals.Count)
        {
            intervalIndex = 0;
            if (loopCount > 0)
                loopCount--;
        }
    }
    public bool TimePassBy()//加bool判定为了可以当普通计时器用
    {
        if (pause) return false;

        if (!isOver)
        {
            if (timer.TimerTick(unscaled))
            {
                action?.Invoke();
                if (haveEvent && !updateInvoke)
                    EventManager.Instance.TriggerEvent(eventName);
                ToNextInterval();
                return true;
            }
            else if (haveEvent && updateInvoke)
                EventManager.Instance.TriggerEvent<int, float>(eventName, loopCount, timer);
        }

        return false;
    }


    public void Clear()
    {
        action = null;
    }
}

