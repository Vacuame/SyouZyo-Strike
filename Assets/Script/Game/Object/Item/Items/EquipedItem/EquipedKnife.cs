using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(BoxCollider))]
public class EquipedKnife : EquipedItem
{
    [HideInInspector]public BoxCollider atkRange;
    public float damage;

    public override void TakeOut(PlayerCharacter user, ExtraSave extra)
    {
        base.TakeOut(user, data);
        data = extra as EquipedItemSave;
        data.equiped = true;

        //需要立刻拿出来，不要播放动画
        user.anim.Play("Empty", user.anim.GetLayerIndex("Arm"));

        MeleeAsset meleeAsset = Resources.Load<MeleeAsset>(user.abilityRootPath+"MeleeData");
        owner.GrandAbility(new Melee(meleeAsset,user,this));

        owner.GrandAbility(new Assassination(Resources.Load<AssassinationAsset>
        (user.abilityRootPath + "AssassinationAsset"), user, user.assasinRange));

        user.controller.control.Player.Fire.started += UseKnife;
    }
    public override void PutIn()
    {
        base.PutIn();
        data.equiped = false;

        user.controller.control.Player.Fire.started -= UseKnife;
        owner.RemoveAbility("Melee");

        GameObject.Destroy(gameObject);
    }
    private void UseKnife(CallbackContext context)
    {
        if (owner.TryActivateAbility("Assassination"))
            return;
        owner.TryActivateAbility("Melee");
    }
        

    private void Awake()
    {
        atkRange = GetComponent<BoxCollider>();
    }
}
