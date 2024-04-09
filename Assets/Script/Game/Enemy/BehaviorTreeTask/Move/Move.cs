using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : EnemyAction
{
    public SharedFloat maxSpeed;
    public SharedFloat stopDistance;

    public override TaskStatus OnUpdate()
    {
        if (maxSpeed.Value != 0)
            me.nav.speed = maxSpeed.Value;
        me.nav.destination = GetTargetPos();
        me.nav.stoppingDistance = GetStopDistance();

        if (me.nav.remainingDistance <= GetStopDistance())
            return TaskStatus.Success;

        return TaskStatus.Running;
    }
    protected abstract Vector3 GetTargetPos();
    protected virtual float GetStopDistance() => stopDistance.Value;

}
