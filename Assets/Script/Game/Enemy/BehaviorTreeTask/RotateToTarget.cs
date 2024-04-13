using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Mine")]
public class RotateToTarget : EnemyAction
{
    public SharedGameObject target;
    public SharedFloat rotateSpeed;

    public override TaskStatus OnUpdate()
    {
        if(target.Value==null)
            return TaskStatus.Failure;

        Vector3 toForward = target.Value.transform.position - transform.position;
        toForward.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(toForward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed.Value * Time.deltaTime);

        if(Quaternion.Angle(transform.rotation, targetRotation)==0)
            return TaskStatus.Success;
        return TaskStatus.Running;
    }
}
