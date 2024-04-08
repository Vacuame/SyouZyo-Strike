using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// 根据输入的按键从物品栏装备相应物品
/// </summary>
public class EquipItem : AbstractAbility
{
    public EquipItem(AbilityAsset setAsset) : base(setAsset)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new EquipItemSpec(this,owner);
    }

    public class EquipItemSpec : TimeLineAbilitySpec
    {
        int weaponType;
        PlayerCharacter character;
        Animator anim;
        Rig chestRig;

        EquipedItem equipedItem;
        public EquipItemSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {

        }

        /// <param name="args">PlayerCharacter , Rig</param>
        public override void ActivateAbility(params object[] args)
        {
            character = args[0] as PlayerCharacter;
            anim = character.anim;
            chestRig = args[1] as Rig;

            weaponType = weaponType == 1 ? 0 : 1;
            //播放动画，实际是动画调用拿出道具的函数
            anim.SetInteger("weaponType", weaponType);
            if(weaponType==1)
                EquipGun();
            else
                UnEquipGun();

            EndSelf();
        }

        public override void EndAbility()
        {
            
        }

        private void EquipGun()
        {
            ItemInfo info = ItemManager.Instance.GetItemInfo(1);
            equipedItem = GameObject.Instantiate(info.equipedItemPrefab,character.RightHandTransform);
            character.equipingItem = equipedItem;
            Gun gun = equipedItem as Gun;
            character.leftFollow = gun.handGuard;
            gun.user = character;

            Aim_SO aimSo = Resources.Load<Aim_SO>("ScriptObjectData/Aim");
            owner.GrandAbility(new Aim(aimSo));
            AbilityAsset shootAsset = Resources.Load<AbilityAsset>("ScriptObjectData/Shoot");
            owner.GrandAbility(new Shoot(shootAsset));
            AbilityAsset reloadAsset = Resources.Load<AbilityAsset>("ScriptObjectData/Reload");
            owner.GrandAbility(new Reload(reloadAsset));

            character.controller.control.Player.Aim.started += AimSt;
            character.controller.control.Player.Aim.canceled += AimEd;
            character.controller.control.Player.Fire.started += ShootSt;
            character.controller.control.Player.Fire.canceled += ShootEd;
            character.controller.control.Player.Reload.started += Reload;

        }
        private void UnEquipGun()
        {
            character.controller.control.Player.Aim.started -= AimSt;
            character.controller.control.Player.Aim.canceled -= AimEd;
            character.controller.control.Player.Fire.started -= ShootSt;
            character.controller.control.Player.Fire.canceled -= ShootEd;
            character.controller.control.Player.Reload.started -= Reload;

            owner.RemoveAbility("Aim");
            owner.RemoveAbility("Shoot");
            owner.RemoveAbility("Reload");

            GameObject.Destroy(equipedItem.gameObject);
        }

        private void AimSt(CallbackContext context) =>
                owner.TryActivateAbility("Aim",character,chestRig);
        private void AimEd(CallbackContext context) =>
            owner.TryEndAbility("Aim");
        private void ShootSt(CallbackContext context) =>
                owner.TryActivateAbility("Shoot", character.equipingItem);
        private void ShootEd(CallbackContext context) =>
            owner.TryEndAbility("Shoot");
        private void Reload(CallbackContext context) =>
            owner.TryActivateAbility("Reload",anim,character.equipingItem);

        public override void InitTimeLine()
        {
            
        }
    }

}
