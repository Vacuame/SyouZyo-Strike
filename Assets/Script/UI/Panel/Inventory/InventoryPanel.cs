using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoleMole;


public partial class InventoryPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("InventoryPanel");

    [SerializeField] private List<InventoryTetris> inventoryTetrisList;

    public InventoryDrager _inventoryDrager { get; private set; }
    public InventoryDrager inventoryDrager;
    protected override void Init()
    {
        inventoryDrager = new InventoryDrager(inventoryTetrisList);

        foreach (var tetris in inventoryTetrisList)
            tetris.Init(this);
    }

    public override void OnExit(bool trueDestroy)
    {
        base.OnExit(trueDestroy);


    }

    private void Update()
    {
        inventoryDrager.Tick();
    }
}
