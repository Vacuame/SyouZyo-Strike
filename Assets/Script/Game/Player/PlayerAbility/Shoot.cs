using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : AbstractAbility
{
    public Shoot(AbilityAsset setAsset) : base(setAsset)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new ShootSpec(this,owner);
    }

    public class ShootSpec : AbilitySpec
    {
        public ShootSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {

        }

        Gun gun;
        /// <param name="args"></param>
        public override void ActivateAbility(params object[] args)
        {
            gun = args[0] as Gun;
            gun.TrySetShooting(true);
        }

        public override void EndAbility()
        {
            gun.TrySetShooting(false);
        }
    }

}
