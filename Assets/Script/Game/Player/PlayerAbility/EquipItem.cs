using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// 根据输入的按键从物品栏装备相应物品
/// </summary>
public class EquipItem : AbstractAbility<EquipItemAsset>
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
        EquipItem equip;
        EquipItemAsset equipAsset => equip.AbilityAsset;

        int weaponType;
        PlayerCharacter character;
        Animator anim;
        Rig chestRig;

        EquipedItem equipedItem;
        ItemSave itemSave;
        ItemInfo itemInfo;

        public EquipItemSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            equip = ability as EquipItem;
            character = equipAsset.character;
            anim = character.anim;
            chestRig = equipAsset.chestRig;
        }

        /// <param name="args">
        /// ItemInfo ItemSave
        /// </param>
        public override void ActivateAbility(params object[] args)
        {
            ItemInfo newItemInfo = args[0] as ItemInfo;
            ItemSave newItemSave = args[1] as ItemSave;

            if(newItemInfo.type == ItemInfo.ItemType.Gun)
            {
                if(itemSave!=null && itemSave!=newItemSave)
                    UnEquipGun(itemSave as GunItemSave);

                GunItemSave gunSave = newItemSave as GunItemSave;
                weaponType = gunSave.equiped? 0:1;
                //播放动画，实际是动画调用拿出道具的函数
                anim.SetInteger("weaponType", weaponType);
                if (!gunSave.equiped)
                    EquipGun(newItemInfo, gunSave);
                else
                    UnEquipGun(gunSave);

                
            }
            EndSelf();
        }

        public override void EndAbility()
        {
            
        }

        private void EquipGun(ItemInfo info,GunItemSave gunSave)
        {
            itemInfo = info;
            itemSave = gunSave;

            equipedItem = GameObject.Instantiate(info.equipedItemPrefab,character.RightHandTransform);
            character.equipingItem = equipedItem;
            Gun gun = equipedItem as Gun;
            gun.curAmmo = gunSave.curAmmo;
            character.leftFollow = gun.handGuard;
            gun.user = character;
            gunSave.equiped = true;

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
        private void UnEquipGun(GunItemSave gunSave)
        {
            gunSave.equiped = false;
            Gun gun = equipedItem as Gun;
            gunSave.curAmmo = gun.curAmmo;

            itemInfo = null;
            itemSave = null;

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
