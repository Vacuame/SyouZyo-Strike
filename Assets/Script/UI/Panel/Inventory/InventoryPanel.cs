using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoleMole;
using System.Runtime.InteropServices;

public class InventoryPanelContext : PanelContext
{
    public ItemSaveData itemSave;
    public ItemSaveData delTetrisData;
    public PlayerController controller;
    public InventoryPanelContext(UIType viewType, PlayerController controller, ItemSaveData itemSave, ItemSaveData delTetrisData) : base(viewType)
    {
        this.itemSave = itemSave;
        this.delTetrisData = delTetrisData;
        this.controller = controller;
    }
}

public class InventoryPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("Inventory/InventoryPanel");
    public static readonly ItemSaveData emptyDelTetrisData = new ItemSaveData(3, 7, new List<ItemSave>());

    public InventoryPanelContext inventoryContext=>context as InventoryPanelContext;

    [SerializeField] private InventoryTetris mainTetris;
    [SerializeField] private InventoryTetris delTetris;
    [HideInInspector]private List<InventoryTetris> inventoryTetrisList = new List<InventoryTetris>();
    [SerializeField] private RectTransform dragContainer;
    [Header("Prefab")]
    [SerializeField] private Transform _tetrisItemPrefab;
    public Transform tetrisItemPrefab { get => _tetrisItemPrefab; }
    public InventoryDrager _inventoryDrager { get; private set; }
    public InventoryDrager inventoryDrager;

    private ItemSaveData itemSave;

    protected override void Init()
    {
        inventoryDrager = new InventoryDrager(inventoryTetrisList,dragContainer);

        inventoryTetrisList.Add(mainTetris);
        inventoryTetrisList.Add(delTetris);
    }
    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);
        itemSave = inventoryContext.itemSave;
        mainTetris.Init(this, itemSave);
        delTetris.Init(this, inventoryContext.delTetrisData);

        inventoryContext.controller.control.Player.Disable();
    }

    public override void OnExit(bool trueDestroy)
    {
        base.OnExit(trueDestroy);
        //TODO 移除在delTetris的东西
        foreach (var a in inventoryTetrisList)
            a.RemoveAllItem();
        inventoryContext.controller.control.Player.Enable();
    }

    private void Update()
    {
        inventoryDrager.Tick();
        if (Input.GetMouseButtonDown(1))
            UIManager.Instance.Pop();
    }
}
