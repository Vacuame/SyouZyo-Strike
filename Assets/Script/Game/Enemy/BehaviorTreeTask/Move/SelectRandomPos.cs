using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

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
        Vector3 randomPos = new Vector3(xzPos.x, center.y, xzPos.y);

        //在Nav中寻找最近的烘焙点
        movePos.Value = NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1f, 1) ? 
            hit.position : transform.position;

        return TaskStatus.Success;
    }
}
