using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
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

        (GameRoot.Instance.gameMode as GameMode_Play)?.OnNotifyPlayer(target.Value);

        return TaskStatus.Success;
    }
}
