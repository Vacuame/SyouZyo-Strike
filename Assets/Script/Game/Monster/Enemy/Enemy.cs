using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PatrolType
{
    ReverseOnEnd,Circle
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public class Enemy : Character
{
    [HideInInspector]public NavMeshAgent nav;
    protected BehaviorTree bt;

    [SerializeField,Header("—≤¬ﬂ…Ë÷√")] private Transform patrolPointList;
    [SerializeField] public PatrolType patrolType;
    [HideInInspector]public List<Transform> patrolPoints;

    protected override void Awake()
    {
        base.Awake();
        bt = GetComponent<BehaviorTree>();
        nav = GetComponent<NavMeshAgent>();
        for(int i=0;i<patrolPointList.childCount;i++)
        {
            patrolPoints.Add(patrolPointList.GetChild(i));
        }
            
    }


    protected override void OnDead()
    {
        
    }

    protected override void OnDeadEnd()
    {
        
    }

    protected override void OnDeadStart()
    {
        
    }
}
