using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Mine/Extension")]
public class PlayAnim : EnemyAction
{
    public AnimPlayConfig config;
    public override TaskStatus OnUpdate()
    {
        config.PlayAnim(me.anim);
        return TaskStatus.Success;
    }
}
