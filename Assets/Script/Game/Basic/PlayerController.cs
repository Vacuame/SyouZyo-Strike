using System.Collections.Generic;
using UnityEngine;
using GameBasic;
using MyUI;

/// <summary>
/// 实际的玩家，不仅控制角色，还有背包等其他功能
/// </summary>
public class PlayerController : Controller
{
    public ItemSaveData itemSaveData;

    public ItemSave equipingItem { get; private set; }
    public void SetEquipingItem(ItemSave item)
    {
        equipingItem = item;
    }
    public ItemSave[] shortCutSlot = new ItemSave[4];

    protected override void Start()
    {
        base.Start();
        itemSaveData = new ItemSaveData(11,7,new List<ItemSave>());
        ItemSave defaultGun = new ItemSave(5, new Vector2Int(0, 5), InventoryStatic.Dir.Down, new EquipedItemSave(10, false));
        itemSaveData.AddItem(defaultGun);
        itemSaveData.AddItem(new ItemSave(3, new Vector2Int(0, 4), InventoryStatic.Dir.Down, new EquipedItemSave(50,false)));

        GameRoot.Instance.GetGameMode<GameMode_Play>().playerController = this;
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Tab))
            UIManager.Instance.SwitchOnPeek(new InventoryPanelContext(InventoryPanel.uiType,this,itemSaveData,InventoryPanel.emptyDelTetrisData));

        if (Input.GetKeyDown(KeyCode.Escape))
            UIManager.Instance.SwitchOnPeek(new PanelContext(PausePanel.uiType));

    }

    public void PickUpNewItem(ItemInfo itemInfo,ExtraSave extra)
    {
        if (!itemSaveData.TryGetNewItem(itemInfo,extra))
        {
            ItemSaveData delTetrisData = InventoryPanel.emptyDelTetrisData;
            InventoryStatic.Dir dir = InventoryStatic.Dir.Down;
            if (itemInfo.width > delTetrisData.bagWidth)
                dir = InventoryStatic.Dir.Left;
            int height = dir == InventoryStatic.Dir.Down ? itemInfo.height : itemInfo.width;

            delTetrisData.AddItem(new ItemSave(
                itemInfo.id, new Vector2Int(0, delTetrisData.bagHeight - height), dir,extra));

            UIManager.Instance.Push(new InventoryPanelContext(InventoryPanel.uiType,this, itemSaveData, delTetrisData));
        }
    }
}


