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

        PlayerController player;
        Animator anim;
        EquipedGun gun;
        float endTimer;

        private List<ItemSave> ammoBoxes = new List<ItemSave>();
        
        /// <param name="args">Anim , Gun</param>
        public override void ActivateAbility(params object[] args)
        {
            anim = args[0] as Animator;
            gun = args[1] as EquipedGun;
            player = args[2] as PlayerController;

            int ammoId = gun.ammoId;
            bool canReload = ammoId != 0 && gun.curAmmo < gun.fullAmmo;

            if (!canReload||!player.itemSaveData.TryGetListOfItem(ammoId, out ammoBoxes))
            {
                EndSelf();
                return;
            }
                
            //int layerIndex = anim.GetLayerIndex("Arm");
            anim.SetTrigger("reload");
            endTimer = 1.9f;
        }

        protected override void AbilityTick()
        {
            if(endTimer.TimerTick())
            {
                //得到子弹列表，然后上子弹
                int needAmmo = gun.fullAmmo - gun.curAmmo;
                int remainNeedAmmo = needAmmo;

                ammoBoxes.Sort((a, b) => { return (a.extra.num > b.extra.num) ? 1 : -1; });

                List<ItemSave>boxToBeEmpty = new List<ItemSave>();

                foreach(var ammoBox in ammoBoxes)
                {
                    if (remainNeedAmmo >= ammoBox.extra.num)
                    {
                        remainNeedAmmo -= ammoBox.extra.num;
                        boxToBeEmpty.Add(ammoBox);//等于0就被移除了，之后再移除
                    }
                    else
                    {
                        ammoBox.extra.num -= remainNeedAmmo;
                        remainNeedAmmo = 0;
                    }
                    if (remainNeedAmmo == 0)
                        break;
                }

                foreach (var a in boxToBeEmpty)
                    a.extra.num = 0;

                int usedAmmo = needAmmo - remainNeedAmmo;
                gun.bagAmmo -= usedAmmo;
                gun.curAmmo += usedAmmo;

                EndSelf();
            }

        }

        public override void EndAbility()
        {
            
        }
    }
}
