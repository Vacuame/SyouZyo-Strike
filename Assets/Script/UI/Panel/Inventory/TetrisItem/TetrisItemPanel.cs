using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisItemPanelContext : PanelContext
{
    public Vector3 rectPos;
    public TetrisItem tetrisItem;
    public ItemInfo itemInfo => tetrisItem.itemInfo;
    public ItemSave itemSave=>tetrisItem.itemSave;
    public PlayerController controller=>tetrisItem.inventoryTetris.inventoryPanel.inventoryContext.controller;
    public TetrisItemPanelContext(UIType viewType, Vector3 rectPos,TetrisItem tetrisItem) : base(viewType)
    {
        this.rectPos = rectPos;
        this.tetrisItem = tetrisItem;
    }
}

public class TetrisItemPanel : BasePanel
{
    TetrisItemPanelContext tetrisItemContext => context as TetrisItemPanelContext;
    public static readonly UIType uiType = new UIType("Inventory/TetrisItemPanel/TetrisItemPanel");
    [SerializeField] private Button btnPrefab;

    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);
        transform.position = tetrisItemContext.rectPos;
        switch (tetrisItemContext.itemInfo.type)
        {
            case ItemInfo.ItemType.Item:
                break;
            case ItemInfo.ItemType.Useable:
                break;
            case ItemInfo.ItemType.Gun:
                Button btnEquip = GameObject.Instantiate(btnPrefab, transform);
                btnEquip.onClick.AddListener(EquipWeapon);
                GunItemSave gunSave = tetrisItemContext.itemSave.extra as GunItemSave;
                string txtUse = gunSave.equiped ? "卸下" : "装备";
                btnEquip.GetComponentInChildren<Text>().text = txtUse;
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
        if(tetrisItemContext.controller.controlledPawn is PlayerCharacter)
        {
            PlayerCharacter character = tetrisItemContext.controller.controlledPawn as PlayerCharacter;
            if(character.ABS.TryActivateAbility("EquipItem",tetrisItemContext.itemInfo,tetrisItemContext.itemSave))
            {
                //设置Save并显示已装备
                GunItemSave gunSave = tetrisItemContext.itemSave.extra as GunItemSave;
                tetrisItemContext.tetrisItem.onUseTip.SetActive(gunSave.equiped);
            }
        }
        UIManager.Instance.Pop();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            UIManager.Instance.Pop();
    }

}
