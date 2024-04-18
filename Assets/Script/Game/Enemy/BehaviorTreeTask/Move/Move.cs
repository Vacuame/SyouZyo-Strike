using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public abstract class Move : EnemyAction
{
    public SharedFloat maxSpeed;
    public SharedFloat stopDistance;

    private bool SetNavDestination;

    public override TaskStatus OnUpdate()
    {
        if (maxSpeed.Value != 0)
            me.nav.speed = maxSpeed.Value;
        me.nav.destination = GetTargetPos();
        me.nav.stoppingDistance = GetStopDistance();

        if (me.nav.path.status == NavMeshPathStatus.PathPartial)
        {
            return TaskStatus.Failure;
        }
            
        if ((transform.position - GetTargetPos()).sqrMagnitude <= GetSqrStopDistance())
        {
            me.nav.destination = transform.position;
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
    protected abstract Vector3 GetTargetPos();
    protected virtual float GetStopDistance() => stopDistance.Value;

    protected float GetSqrStopDistance() => Mathf.Pow(GetStopDistance(), 2);
}
