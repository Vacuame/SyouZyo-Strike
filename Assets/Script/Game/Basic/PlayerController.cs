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
        ItemSave testGun = new ItemSave(1, new Vector2Int(2, 4), InventoryStatic.Dir.Down, new EquipedItemSave(24, false));
        itemSaveData.AddItem(testGun);
        //shortCutSlot[0] = testGun;
        itemSaveData.AddItem(new ItemSave(2, new Vector2Int(0, 0), InventoryStatic.Dir.Down,new ExtraSave(1)));
        itemSaveData.AddItem(new ItemSave(3, new Vector2Int(2, 0), InventoryStatic.Dir.Down, new EquipedItemSave(50,false)));
        itemSaveData.AddItem(new ItemSave(4, new Vector2Int(8, 6), InventoryStatic.Dir.Down, new ExtraSave(37)));
        itemSaveData.AddItem(new ItemSave(4, new Vector2Int(8, 5), InventoryStatic.Dir.Down, new ExtraSave(12)));
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Tab))
            UIManager.Instance.SwitchOnPeek(new InventoryPanelContext(InventoryPanel.uiType,this,itemSaveData,InventoryPanel.emptyDelTetrisData));

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


