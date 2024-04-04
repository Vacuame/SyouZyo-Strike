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
    private ItemSave itemSave;

    protected override void Start()
    {
        base.Start();
        itemSave = new ItemSave();
        itemSave.bagWidth = 11; itemSave.bagHeight = 7;
        itemSave.items.Add(new TetrisInfo(1, new Vector2Int(2, 4),InventoryStatic.Dir.Down));
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
            UIManager.Instance.SwitchOnPeek(new InventoryPanelContext(InventoryPanel.uiType,itemSave));
    }

}


