using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Mine")]
public class LocomotionAnim : EnemyAction
{
    public override TaskStatus OnUpdate()
    {
        float moveSpeed = me.nav.velocity.magnitude;
        me.anim.SetFloat("MoveSpd", moveSpeed);
        Debug.Log(moveSpeed);
        return TaskStatus.Running;
    }
}
