using UnityEngine;
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
        (user.abilityRootPath + "AssassinationAsset"), user, user.assasinRange,transform));

        owner.GrandAbility(new Parry(Resources.Load<ParryAsset>
        (user.abilityRootPath + "Parry/ParryAsset"), user));

        user.controller.control.Player.Fire.started += UseKnife;
        user.controller.control.Player.Block.started += TryStartParry;
        user.controller.control.Player.Block.canceled += TryEndParry;
    }
    public override void PutIn()
    {
        base.PutIn();
        data.equiped = false;

        user.controller.control.Player.Fire.started -= UseKnife;
        user.controller.control.Player.Block.started -= TryStartParry;
        user.controller.control.Player.Block.canceled -= TryEndParry;

        owner.RemoveAbility("Melee");
        owner.RemoveAbility("Assassination");
        owner.RemoveAbility("Parry");

        GameObject.Destroy(gameObject);
    }
    private void UseKnife(CallbackContext context)
    {
        if (owner.TryActivateAbility("Assassination"))
            return;
        owner.TryActivateAbility("Melee");
    }

    private void TryStartParry(CallbackContext context)
    {
        owner.TryActivateAbility("Parry");
    }
    private void TryEndParry(CallbackContext context)
    {
        owner.TryEndAbility("Parry");
    }

    private void Awake()
    {
        atkRange = GetComponent<BoxCollider>();
    }
}
