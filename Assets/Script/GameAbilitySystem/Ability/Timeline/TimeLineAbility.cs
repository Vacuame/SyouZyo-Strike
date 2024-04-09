using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeLineAbilitySpec : AbilitySpec
{
    protected SkillTimeLine timeLine = new SkillTimeLine();
    public TimeLineAbilitySpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
    {

    }

    public abstract void InitTimeLine();

    public override void ActivateAbility(params object[] args)
    {
        timeLine.Start();
    }

    protected override void AbilityTick()
    {
        timeLine.Update();
    }

    public override void EndAbility()
    {
        
    }
}

