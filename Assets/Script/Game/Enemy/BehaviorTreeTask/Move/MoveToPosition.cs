using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Mine")]
public class MoveToPosition : Move
{
    public SharedVector3 movePos;

    public override TaskStatus OnUpdate()
    {
        if(movePos.Value == Consts.NullV3)
            return TaskStatus.Failure;

        return base.OnUpdate();
    }

    protected override Vector3 GetTargetPos()
    {
        return movePos.Value;
    }
}
