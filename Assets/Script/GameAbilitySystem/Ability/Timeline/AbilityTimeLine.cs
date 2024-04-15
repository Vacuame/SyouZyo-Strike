using System;
using UnityEngine;

public class AbilityTimeLine
{
    private bool bStart;//是否开始
    public bool bPause;//是否暂停
    private float curTime;//当前计时    
    private Action reset;//重置事件
    private Action<float> update;//每帧回调

    public AbilityTimeLine()
    {
        Reset();
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="delay">延迟时间</param>
    /// <param name="id">ID（穿透参数）</param>
    /// <param name="method">执行的回调</param>
    public void AddEvent(float delay, Action method)
    {
        LineEvent param = new LineEvent(delay,method);
        update += param.Invoke;//条件判断
        reset += param.Reset;
    }

    //开始时间线
    public void Start()
    {
        Reset();
        bStart = true;
        bPause = false;
    }

    //重置（还原）
    public void Reset()
    {
        curTime = 0;//时间线计时归零
        bStart = false;//不开始
        bPause = false;//没开始就不用谈暂停

        if (null != reset)
        {
            reset();//在时间线的LineEvent里面去调用所有事件（Event）的reset函数，所有的时间线事件（LineEvent）也要归零
        }
    }

    public void Update()
    {
        if (!bStart || bPause)//时间线开始并且没有被暂停就进入下面
        {
            return;
        }
        float deltaTime = Time.deltaTime;
        curTime += deltaTime;
        if (null != update)
        {
            update(curTime);
        }
    }


    /* ==================附属的类LineEvent================== */
    private class LineEvent
    {
        public float Delay { get; protected set; }//延迟时间
        public Action Method { get; protected set; }//回调函数
        public bool isInvoke = false;

        public LineEvent(float delay,Action method)
        {
            Delay = delay;
            Method = method;
            Reset();//重置各种状态
        }

        public void Reset()
        {
            isInvoke = false;
        }

        //每帧执行（自己驱动的帧）,time是从时间线开始，到目前为止经过的时间
        public void Invoke(float time)
        {
            //当前事件还没到延迟时间，直接返回
            if (time < Delay)
            {
                return;
            }
            if (!isInvoke && null != Method)
            {
                isInvoke = true;
                Method();//保证Method在时间线的整个生存周期内只会执行一次
            }
        }
    }


}