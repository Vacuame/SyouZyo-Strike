using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[TaskCategory("Mine")]
public class NormalAttackTask : EnemyAction
{
    public override TaskStatus OnUpdate()
    {
        if(me.ABS.TryActivateAbility("Attack", me))
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
