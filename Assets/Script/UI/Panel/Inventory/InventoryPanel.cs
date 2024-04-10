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
    }

    //TODO 逻辑有点怪，物体数量可以直接修改存档，但这里直接清空存档重新保存就为了获得位置信息，有空改一下
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
        if (Input.GetMouseButtonDown(1))
            UIManager.Instance.Pop();
    }
}
