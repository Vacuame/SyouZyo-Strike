using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : EnemyAction
{
    public SharedGameObject target;

    public SharedFloat Test_stopDistance;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
            return TaskStatus.Failure;

        me.nav.destination = target.Value.transform.position;
        me.nav.stoppingDistance = Test_stopDistance.Value;

        if(Vector3.Distance(me.transform.position,target.Value.transform.position)<= Test_stopDistance.Value)
            return TaskStatus.Success;
            
        return TaskStatus.Running;
    }

}
