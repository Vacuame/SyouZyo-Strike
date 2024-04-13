using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Mine")]
public class Look : EnemyConditional
{
    public SharedGameObject target;
    public SharedBool onFindTargetSetBool;
    private ConeDetector eye;

    public override void OnAwake()
    {
        base.OnAwake();
        eye = transform.GetComponentInChildren<ConeDetector>();

    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)   //没有才去找
        {
            if (eye.TryDetect(out GameObject g))
            {
                target.Value = g;
                onFindTargetSetBool.Value = true;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }
}


