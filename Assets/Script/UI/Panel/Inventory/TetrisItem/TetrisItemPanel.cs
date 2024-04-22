using MoleMole;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class TetrisItemPanelContext : PanelContext
{
    public Vector3 rectPos;
    public TetrisItem tetrisItem;
    public TetrisItemPanelContext(UIType uiType, Vector3 rectPos,TetrisItem tetrisItem) : base(uiType)
    {
        this.rectPos = rectPos;
        this.tetrisItem = tetrisItem;
    }
}

public class TetrisItemPanel : BasePanel
{
    TetrisItemPanelContext tetrisItemContext => context as TetrisItemPanelContext;
    public static readonly UIType uiType = new UIType("Inventory/TetrisItem/TetrisItemPanel");
    [SerializeField] private Button btnPrefab;

    public ItemInfo itemInfo => tetrisItemContext.tetrisItem.itemInfo;
    public ItemSave itemSave => tetrisItemContext.tetrisItem.itemSave;
    public PlayerController controller => tetrisItemContext.tetrisItem.inventoryTetris.inventoryPanel.inventoryContext.controller;
    PlayerCharacter character => controller.controlledPawn as PlayerCharacter;

    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);
        transform.position = tetrisItemContext.rectPos;
        switch (itemInfo.type)
        {
            case ItemInfo.ItemType.Item:
                break;
            case ItemInfo.ItemType.Useable:
                Button btnUse = GameObject.Instantiate(btnPrefab, transform);
                btnUse.onClick.AddListener(UseItem);
                btnUse.GetComponentInChildren<Text>().text = "使用";
                break;
            case ItemInfo.ItemType.Knife:
            case ItemInfo.ItemType.Gun:
                Button btnEquip = GameObject.Instantiate(btnPrefab, transform);
                btnEquip.onClick.AddListener(EquipWeapon);
                EquipedItemSave gunSave = itemSave.extra as EquipedItemSave;
                string txtUse = gunSave.equiped ? "卸下" : "装备";
                btnEquip.GetComponentInChildren<Text>().text = txtUse;

                Button btnSlot = GameObject.Instantiate(btnPrefab, transform);
                btnSlot.onClick.AddListener(ToSetSlot);
                btnSlot.GetComponentInChildren<Text>().text = "快捷键";
                break;
        }
    }

    public override void OnExit(bool trueDestroy)
    {
        base.OnExit(trueDestroy);
        for (int i = 0; i < transform.childCount; i++)
            GameObject.Destroy(transform.GetChild(i).gameObject);
    }

    private void EquipWeapon()
    {
        if(controller.controlledPawn is PlayerCharacter)
        {
            if(character.ABS.TryActivateAbility("EquipItem",itemInfo,itemSave))
            {
                //设置Save并显示已装备
                EquipedItemSave gunSave = itemSave.extra as EquipedItemSave;
                tetrisItemContext.tetrisItem.onUseTip.SetActive(gunSave.equiped);
            }
        }
        UIManager.Instance.Pop();
    }
    private void ToSetSlot()
    {
        UIManager.Instance.Pop();
        UIManager.Instance.Push(new SetSlotPanelContext(SetSlotPanel.uiType, controller, itemSave));
    }

    private void UseItem()
    {
        UseableItemInfo useInfo = itemInfo as UseableItemInfo;
        foreach(var asset in useInfo.effectAssets)
        {
            character.ABS.ApplyGameplayEffectToSelf(new GameplayEffect(asset));
        }
        itemSave.extra.num--;
        UIManager.Instance.Pop();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            UIManager.Instance.Pop();
    }

}
