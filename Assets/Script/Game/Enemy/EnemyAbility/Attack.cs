using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : AbstractAbility<Attack_SO>
{
    public Attack(AbilityAsset setAsset) : base(setAsset)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new AttackSpec(this,owner);
    }

    public class AttackSpec : TimeLineAbilitySpec
    {
        Character me;

        Attack attack;
        Attack_SO asset => attack.AbilityAsset;

        public AttackSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            attack = ability as Attack;
        }

        public override void ActivateAbility(params object[] args)
        {
            me = args[0] as Character;
            base.ActivateAbility(args);
        }

        public override void InitTimeLine()
        {
            timeLine.AddEvent(0, PlayAnim);
            timeLine.AddEvent(2, EndSelf);
        }

        private void PlayAnim()
        {
            me.anim.Play("Attack", 0);
        }

    }


}
