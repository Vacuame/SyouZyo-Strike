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

        inventoryContext.controller.itemSaveData.onItemAdded += OnItemAdded;
        inventoryContext.controller.itemSaveData.onItemRemoved += OnItemRemoved;

        inventoryContext.controller.control.Player.Disable();
        inventoryContext.controller.mouseSpeedMul = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void OnExit(bool trueDestroy)
    {
        base.OnExit(trueDestroy);
        RemoveItemInDelTetris();

        foreach (var a in inventoryTetrisList)
            a.RemoveAllItem();

        inventoryContext.controller.itemSaveData.onItemAdded -= OnItemAdded;
        inventoryContext.controller.itemSaveData.onItemRemoved -= OnItemRemoved;

        inventoryContext.controller.control.Player.Enable();
        inventoryContext.controller.mouseSpeedMul = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        inventoryDrager.Tick();
        if (Input.GetMouseButtonDown(1))
            UIManager.Instance.Pop();
    }


    private void OnItemAdded(ItemSave item)
    {
        mainTetris.TryPlaceNewItem(item, item.pos, item.dir);
    }
    private void OnItemRemoved(ItemSave item)
    { 
        mainTetris.RemoveItemAt(item.pos);
    }
    private void RemoveItemInDelTetris()
    {
        var grid = delTetris.GetGrid();
        for (int x = 0; x < grid.GetWidth(); x++)
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                TetrisItem item = grid.GetGridObject(x,y).GetItem();

                if (item != null)
                {
                    item.itemSave.extra.num = 0;
                }
            }
    }
}
