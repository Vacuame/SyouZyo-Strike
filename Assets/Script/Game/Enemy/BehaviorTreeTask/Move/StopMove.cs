using BehaviorDesigner.Runtime.Tasks;


[TaskCategory("Mine")]
public class StopMove : EnemyAction
{
    public override TaskStatus OnUpdate()
    {
        me.nav.destination = transform.position;
        return TaskStatus.Success;
    }
}
