using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[TaskCategory("Mine")]
public class NotifyPlayer : EnemyAction
{
    public SharedFloat soundDistance;
    public SharedGameObject target;
    public override TaskStatus OnUpdate()
    {
        if(target.Value == null)
            return TaskStatus.Failure;

        SoundMaker.Instance.MakeSound(me.transform.position,
            new SoundConfig(soundDistance.Value, LayerMask.GetMask("Enemy")),
            new SoundInfo(SoundType.NotifyPlayer, target.Value));
        return TaskStatus.Success;
    }
}
