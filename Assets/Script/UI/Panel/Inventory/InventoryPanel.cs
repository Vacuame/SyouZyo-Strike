using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoleMole;
using System.Runtime.InteropServices;

public class InventoryPanelContext : PanelContext
{
    public TetrisData itemSave;
    public TetrisData delTetrisData;
    public PlayerController controller;
    public InventoryPanelContext(UIType viewType, PlayerController controller, TetrisData itemSave, TetrisData delTetrisData) : base(viewType)
    {
        this.itemSave = itemSave;
        this.delTetrisData = delTetrisData;
        this.controller = controller;
    }
}

public class InventoryPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("InventoryPanel");
    public static readonly TetrisData emptyDelTetrisData = new TetrisData(3, 7, new List<TetrisInfo>());

    [SerializeField] private InventoryTetris mainTetris;
    [SerializeField] private InventoryTetris delTetris;
    [HideInInspector]private List<InventoryTetris> inventoryTetrisList = new List<InventoryTetris>();
    [SerializeField] private RectTransform dragContainer;

    public InventoryDrager _inventoryDrager { get; private set; }
    public InventoryDrager inventoryDrager;

    private TetrisData itemSave;

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
        delTetris.Init(this, inventoryContext.delTetrisData);
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
