using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public abstract class Move : EnemyAction
{
    public SharedFloat moveSpeed;
    public SharedFloat stopDistance;

    public SharedBool randomSpeed;
    public SharedFloat minSpeed;
    public SharedFloat maxSpeed;
    //private bool SetNavDestination;

    public override void OnStart()
    {
        if (randomSpeed.Value)
        {
            moveSpeed.Value = Random.Range(minSpeed.Value, maxSpeed.Value); 
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (moveSpeed.Value != 0)
            me.nav.speed = moveSpeed.Value;
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
