using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;

public class Aim : AbstractAbility<Aim_SO>
{
    public Aim(AbilityAsset setAsset) : base(setAsset)
    {
        
    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new AimSpec(this,owner);
    }

    public class AimSpec : AbilitySpec
    {
        Aim aim;
        Aim_SO asset => aim.AbilityAsset;

        
        public AimSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            aim = ability as Aim;
        }

        PlayerCharacter character;
        Animator anim;
        Rig chestRig;
        /// <param name="args">PlayerCharacter , Rig</param>
        public override void ActivateAbility(params object[] args)
        {
            character = args[0] as PlayerCharacter;
            anim = character.anim;
            chestRig = args[1] as Rig;

            character.controller.playCamera.SwitchCamera(Tags.Camera.Aim);
            chestRig.weight = 1;
            anim.SetBool("aiming", true);
            character.curGun.SetAiming(true);
        }

        protected override void AbilityTick()
        {
            character.curGun.moving = (character.cc.velocity != Vector3.zero);
        }

        public override void EndAbility()
        {
            character.controller.playCamera.SwitchCamera(Tags.Camera.Normal);
            chestRig.weight = 0;
            anim.SetBool("aiming", false);
            character.curGun.SetAiming(false);
        }
    }

}
