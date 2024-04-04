using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoleMole;

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
    private List<InventoryTetris> inventoryTetrisList = new List<InventoryTetris>();
    [SerializeField] private RectTransform dragContainer;

    public InventoryDrager _inventoryDrager { get; private set; }
    public InventoryDrager inventoryDrager;
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
        mainTetris.Init(this, inventoryContext.itemSave);
        delTetris.Init(this);
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
