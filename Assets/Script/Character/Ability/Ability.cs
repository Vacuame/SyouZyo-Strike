using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static Control;

public abstract class Ability
{
    public int priority;

    protected Character caster;//拥有此能力的人
    protected AbilityMgr abilityMgr => caster.abilityMgr;

    protected SkillTimeLine timeLine = new SkillTimeLine();//时间线，给固定动作用的

    public virtual void OnAdd(Character caster,int priority = 0)
    {
        this.caster = caster;
        this.priority = priority;
        //初始化时间线
        InitTimeLine();
    }
    protected abstract void InitTimeLine();//给TimeLine添加事件

    public virtual void OnRemove()
    {

    }


    public virtual bool CanStart()
    {
        return true;
    }
    public virtual void Start()//许多技能会在这里开始timeLine
    {

    }

    public virtual void Update()//持续
    {
        timeLine.Update(Time.deltaTime);
    }

/*    public abstract void HandleInput(PlayerActions input)
    {

    }*/

    public virtual void OnFocusStart()
    {
        //比如Aim会执行相机切换
    }
    public virtual void OnFocusUpdate(PlayerActions input)
    {
        
    }
    public virtual void OnFocusEnd() 
    { 

    }

}
