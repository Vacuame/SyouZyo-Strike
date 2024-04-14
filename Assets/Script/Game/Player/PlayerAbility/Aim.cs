using UnityEngine;
using UnityEngine.Animations.Rigging;

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
        EquipedGun gun;
        Animator anim;
        Rig chestRig;
        /// <param name="args">PlayerCharacter , Rig</param>
        public override void ActivateAbility(params object[] args)
        {
            character = args[0] as PlayerCharacter;
            anim = character.anim;
            chestRig = args[1] as Rig;
            gun = args[2] as EquipedGun;

            character.controller.playCamera.SwitchCamera(Tags.Camera.Aim);
            chestRig.weight = 1;
            anim.SetBool("aiming", true);
            gun.SetAiming(true);
        }

        protected override void AbilityTick()
        {
            Transform cameraTrans = character.controller.playCamera.transform;
            Vector3 cameraAngle = new Vector3(cameraTrans.forward.x, 0, cameraTrans.forward.z);
            Quaternion targetRotation = Quaternion.LookRotation(cameraAngle);

            if(Vector3.Angle(cameraAngle,character.transform.forward)>asset.maxArmAngle)
                character.transform.rotation = targetRotation;
            else
                character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, asset.aimRotateSpeed * Time.fixedDeltaTime);

        }

        public override void EndAbility()
        {
            character.controller.playCamera.SwitchCamera(Tags.Camera.Normal);
            chestRig.weight = 0;
            anim.SetBool("aiming", false);
            gun.SetAiming(false);
        }
    }

}
