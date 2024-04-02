using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;

[TaskCategory("Mine")]
public class MoveToTarget : Move
{
    public SharedGameObject target;
    public override void OnStart()
    {
        //读攻击的信息
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
            return TaskStatus.Failure;

        return base.OnUpdate();
    }

    protected override Vector3 GetTargetPos()
    {
        return target.Value.transform.position;
    }
}
