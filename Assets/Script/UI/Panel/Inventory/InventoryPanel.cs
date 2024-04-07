using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoleMole;
using System.Runtime.InteropServices;

public class InventoryPanelContext : PanelContext
{
    public ItemSave itemSave;
    public InventoryPanelContext(UIType viewType, ItemSave itemSave) : base(viewType)
    {
        this.itemSave = itemSave;
    }
}

public class InventoryPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("InventoryPanel");

    [SerializeField] private InventoryTetris mainTetris;
    [SerializeField] private InventoryTetris delTetris;
    [HideInInspector]private List<InventoryTetris> inventoryTetrisList = new List<InventoryTetris>();
    [SerializeField] private RectTransform dragContainer;

    public InventoryDrager _inventoryDrager { get; private set; }
    public InventoryDrager inventoryDrager;

    private ItemSave itemSave;

    protected override void Init()
    {
        inventoryDrager = new InventoryDrager(inventoryTetrisList,dragContainer);

        inventoryTetrisList.Add(mainTetris);
        inventoryTetrisList.Add(delTetris);
    }
    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);
        InventoryPanelContext inventoryContext = context as InventoryPanelContext;
        itemSave = inventoryContext.itemSave;
        mainTetris.Init(this, itemSave);
        delTetris.Init(this);
    }

    public override void OnExit(bool trueDestroy)
    {
        base.OnExit(trueDestroy);
        itemSave.items = mainTetris.GetItemInfoList();
        foreach (var a in inventoryTetrisList)
            a.RemoveAllItem();
    }

    private void Update()
    {
        inventoryDrager.Tick();
    }
}
