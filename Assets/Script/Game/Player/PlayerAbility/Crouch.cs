using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : AbstractAbility
{
    public Crouch(AbilityAsset setAsset, params object[] binds) : base(setAsset, binds)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new CrouchSpec(this, owner);
    }

    public class CrouchSpec : AbilitySpec
    {
        Character character;
        Animator anim => character.anim;
        public CrouchSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            character = ability.binds[0] as Character;
        }

        public override void ActivateAbility(params object[] args)
        {
            anim.SetBool("crouch", true);
        }

        public override void EndAbility()
        {
            anim.SetBool("crouch", false);
        }
    }

}
