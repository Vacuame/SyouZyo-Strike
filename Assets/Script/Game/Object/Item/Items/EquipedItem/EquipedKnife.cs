using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(BoxCollider))]
public class EquipedKnife : EquipedItem
{
    [HideInInspector]public BoxCollider atkRange;
    public float damage;

    public override void TakeOut(PlayerCharacter user, ExtraSave data)
    {
        base.TakeOut(user, data);

        //需要立刻拿出来，不要播放动画
        user.anim.Play("Empty", user.anim.GetLayerIndex("Arm"));

        MeleeAsset meleeAsset = Resources.Load<MeleeAsset>(user.abilityRootPath+"MeleeData");
        owner.GrandAbility(new Melee(meleeAsset,user,this));

        user.controller.control.Player.Fire.started += Melee;
    }
    public override void PutIn()
    {
        base.PutIn();

        user.controller.control.Player.Fire.started -= Melee;
        owner.RemoveAbility("Melee");

        GameObject.Destroy(gameObject);
    }
    private void Melee(CallbackContext context) =>
        owner.TryActivateAbility("Melee");

    private void Awake()
    {
        atkRange = GetComponent<BoxCollider>();
    }
}
