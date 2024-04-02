using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : Action
{
    protected Enemy me;
    protected CharacterController cc;
    protected TaskStatus taskStatus;
    public void SetTaskStatus(TaskStatus newOne)
    {
        taskStatus = newOne;
    }

    public override void OnAwake()
    {
        me = GetComponent<Enemy>();
        cc = me.cc;
    }
}

