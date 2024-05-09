using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Mine")]
public class SelectPatrolPos : EnemyAction
{
    int curPatrolIndex = 0;
    int indexDirection = 1;
    public SharedVector3 movePos;

    public override TaskStatus OnUpdate()
    {
        //����������
        if (me.patrolPoints == null||me.patrolPoints.Count == 0)
        {
            movePos.Value = me.transform.position;
        }
        else if(me.patrolPoints.Count == 1)
        {
            movePos.Value = me.patrolPoints[0].position;
        }
        //���������ʽѡ��
        else
        {
            movePos.Value = me.patrolPoints[curPatrolIndex].position;

            if ((curPatrolIndex == 0 && indexDirection == -1) ||
                (curPatrolIndex == me.patrolPoints.Count - 1 && indexDirection == 1))
            {
                switch (me.patrolType)
                {
                    case PatrolType.ReverseOnEnd:
                        indexDirection *= -1;
                        break;
                    case PatrolType.Circle:
                        curPatrolIndex = 0;
                        break;
                }
            }

            curPatrolIndex += indexDirection;
        }
        
        return TaskStatus.Success;
    }

}
