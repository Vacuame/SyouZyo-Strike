using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[TaskCategory("Mine")]
public class Attack_Task : EnemyAction
{
    public string attackAbilityName;
    bool tryActived;
    AbilitySpec abilitySpec;
    public override void OnStart()
    {
        tryActived = false;
        abilitySpec = null;
    }
    public override TaskStatus OnUpdate()
    {
        if (!tryActived)
        {
            tryActived = true;
            if (!me.ABS.TryActivateAbility(attackAbilityName))
                return TaskStatus.Failure;
            me.ABS.AbilityContainer.TryGetAbility(attackAbilityName, out abilitySpec);
        }
        else if (!abilitySpec.IsActive)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
