using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBasic;
using MoleMole;

/// <summary>
/// 实际的玩家，不仅控制角色，还有背包等其他功能
/// </summary>
public class PlayerController : Controller
{
    private ItemSaveData itemSaveData;

    protected override void Start()
    {
        base.Start();
        itemSaveData = new ItemSaveData(11,7,new List<ItemSave>());
        itemSaveData.items.Add(new ItemSave(1, new Vector2Int(2, 4),InventoryStatic.Dir.Down,new EquipedItemSave(24, false),itemSaveData));
        itemSaveData.items.Add(new ItemSave(2, new Vector2Int(0, 0), InventoryStatic.Dir.Down,new ExtraSave(1), itemSaveData));
        itemSaveData.items.Add(new ItemSave(3, new Vector2Int(2, 0), InventoryStatic.Dir.Down, new EquipedItemSave(50,false), itemSaveData));
    }

    protected override void ControlPawn(Pawn pawn)
    {
        base.ControlPawn(pawn);

        if (pawn is PlayerCharacter)
            ControllPlayer(pawn as PlayerCharacter);
    }

    private void ControllPlayer(PlayerCharacter character)
    {
        
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Tab))
            UIManager.Instance.SwitchOnPeek(new InventoryPanelContext(InventoryPanel.uiType,this,itemSaveData,InventoryPanel.emptyDelTetrisData));

/*        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            ItemInfo itemInfo = ItemManager.Instance.GetItemInfo(1);
            GetNewItem(itemInfo);
        }*/
    }

    public void GetNewItem(ItemInfo itemInfo,ExtraSave extra)
    {
        if (!itemSaveData.TryGetNewItem(itemInfo,extra))
        {
            ItemSaveData delTetrisData = InventoryPanel.emptyDelTetrisData;
            InventoryStatic.Dir dir = InventoryStatic.Dir.Down;
            if (itemInfo.width > delTetrisData.bagWidth)
                dir = InventoryStatic.Dir.Left;
            int height = dir == InventoryStatic.Dir.Down ? itemInfo.height : itemInfo.width;

            delTetrisData.items.Add(new ItemSave(
                itemInfo.id, new Vector2Int(0, delTetrisData.bagHeight - height), dir,extra,null));

            UIManager.Instance.Push(new InventoryPanelContext(InventoryPanel.uiType,this, itemSaveData, delTetrisData));
        }
    }
}


