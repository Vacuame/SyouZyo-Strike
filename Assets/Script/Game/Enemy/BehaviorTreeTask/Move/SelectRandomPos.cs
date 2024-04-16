using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Mine")]
public class SelectRandomPos : EnemyAction
{
    public SharedVector3 movePos;
    public SharedVector3 placeToCheck;
    public SharedFloat checkRadius;

    public override TaskStatus OnUpdate()
    {
        Vector3 center = placeToCheck.Value;
        Vector2 xzPos = new Vector2 (center.x, center.z);
        xzPos = Calc.CircleRandomPoint(xzPos, checkRadius.Value);
        movePos.Value = new Vector3(xzPos.x, center.y, xzPos.y);

        return TaskStatus.Success;
    }
}
