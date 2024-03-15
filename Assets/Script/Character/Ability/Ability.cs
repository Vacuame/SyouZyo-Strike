using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static Control;

public abstract class Ability
{
    public int priority;

    protected Character caster;//ӵ�д���������
    protected AbilityMgr abilityMgr => caster.abilityMgr;

    protected SkillTimeLine timeLine = new SkillTimeLine();//ʱ���ߣ����̶������õ�

    public virtual void OnAdd(Character caster,int priority = 0)
    {
        this.caster = caster;
        this.priority = priority;
        //��ʼ��ʱ����
        InitTimeLine();
    }
    protected abstract void InitTimeLine();//��TimeLine����¼�

    public virtual void OnRemove()
    {

    }


    public virtual bool CanStart()
    {
        return true;
    }
    public virtual void Start()//��༼�ܻ������￪ʼtimeLine
    {

    }

    public virtual void Update()//����
    {
        timeLine.Update(Time.deltaTime);
    }

/*    public abstract void HandleInput(PlayerActions input)
    {

    }*/

    public virtual void OnFocusStart()
    {
        //����Aim��ִ������л�
    }
    public virtual void OnFocusUpdate(PlayerActions input)
    {
        
    }
    public virtual void OnFocusEnd() 
    { 

    }

}
