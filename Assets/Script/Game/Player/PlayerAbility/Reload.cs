using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : AbstractAbility
{
    public Reload(AbilityAsset setAsset) : base(setAsset)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new ReloadSpec(this,owner);
    }

    public class ReloadSpec : AbilitySpec
    {
        public ReloadSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {

        }

        Animator anim;
        Gun gun;
        float endTimer;
        
        /// <param name="args">Anim , Gun</param>
        public override void ActivateAbility(params object[] args)
        {
            anim = args[0] as Animator;
            gun = args[1] as Gun;

            int layerIndex = anim.GetLayerIndex("Arm");
            anim.Play("Reload", layerIndex);
            endTimer = 1.9f;
        }

        protected override void AbilityTick()
        {
            if(endTimer.TimerTick())
            {
                gun.curAmmo = gun.fullAmmo;
                EndSelf();
            }
                
        }

        public override void EndAbility()
        {
            
        }
    }
}
