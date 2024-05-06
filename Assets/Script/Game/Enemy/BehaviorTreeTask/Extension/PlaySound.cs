using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
[TaskCategory("Mine/Extension/")]
public class PlaySound : EnemyAction
{
    public AudioClip clip;

    public override TaskStatus OnUpdate()
    {
        SoundManager.GetOrCreateInstance()?.PlaySound(SoundPoolType.SFX, clip, me.transform.position);

        return TaskStatus.Success;
    }
}
