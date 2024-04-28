using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryStatic;
using static InventoryTetris;

[RequireComponent(typeof(CanvasGroup))]
public class TetrisItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerClickHandler
{
    public ItemInfo itemInfo;
    public ItemSave itemSave;

    [HideInInspector] public Dir dir;
    [HideInInspector] public Vector2Int gridPos;

    [Header("Visual")]
    [SerializeField] private Image block;
    [SerializeField] private List<Pair<DragState, Color>> blockColorSetting;
    [SerializeField] private Image img;
    public GameObject onUseTip;
    public GameObject numTip;
    [HideInInspector]public Text txtNum;
    private CanvasGroup canvasGroup;
    //TODO 如果在deltetris则不可打开选单
    //public bool useable;

    public InventoryTetris inventoryTetris { get; private set; }
    private InventoryDrager InventoryDrager => inventoryTetris.inventoryPanel.inventoryDrager;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        txtNum = numTip.GetComponentInChildren<Text>();
    }
    public void SetInfo(ItemInfo itemInfo, ItemSave itemSave)
    {
        this.itemInfo = itemInfo;
        this.itemSave = itemSave;

        itemSave.extra.onNumChanged += OnNumChanged;

        switch(itemInfo.type)
        {
            case ItemInfo.ItemType.Gun:
            case ItemInfo.ItemType.Knife:
                EquipedItemSave equipSave = itemSave.extra as EquipedItemSave;
                onUseTip.SetActive(equipSave.equiped);
                if(itemInfo.type == ItemInfo.ItemType.Gun)
                {
                    numTip.SetActive(true);
                    txtNum.text = equipSave.durability.ToString();
                }
                equipSave.onEquipedChanged += OnEquipedChanged;
                break;
            case ItemInfo.ItemType.Item:
            case ItemInfo.ItemType.Useable:
                if(itemInfo.maxStackNum>1)
                {
                    numTip.SetActive(true);
                    txtNum.text = itemSave.extra.num.ToString();
                }
                break;
        }
    }

    #region OnItemSaveChange
    public void OnEquipedChanged(bool eq)
    {
        if (onUseTip!=null)
        {
            onUseTip.SetActive(eq);
        }
    }
    public void OnNumChanged(int num)
    {
        txtNum.text = num.ToString();

        if (num <= 0)
            inventoryTetris.RemoveItemAt(gridPos);
    }
    #endregion

    private void OnDestroy()
    {
        itemSave.extra.onNumChanged -= OnNumChanged;
        switch (itemInfo.type)
        {
            case ItemInfo.ItemType.Gun:
                EquipedItemSave gunSave = itemSave.extra as EquipedItemSave;
                gunSave.onEquipedChanged -= OnEquipedChanged;
                break;
        }
    }
    public void SetTetris(Vector2Int gridPos, Dir dir, InventoryTetris inventoryTetris)
    {
        this.gridPos = itemSave.pos = gridPos;
        this.dir = itemSave.dir = dir;
        this.inventoryTetris = inventoryTetris;

        transform.SetParent(inventoryTetris.GetItemContainer());
        transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(dir));
        Grid<ItemBlock> grid = inventoryTetris.GetGrid();
        Vector3 anchoredPostion = grid.GetTransformPosition(gridPos.x, gridPos.y) +
                       (Vector3)GetRotationOffset(itemInfo, dir) * grid.GetCellSize() / 2;

        if (_imgCellSize != grid.GetCellSize())
            adjustVisualSize(grid.GetCellSize());

        transform.localPosition = anchoredPostion;
    }
    public List<Vector2Int> GetGridPositionList()
    {
        return InventoryStatic.GetGridPositionList(gridPos, dir, itemInfo);
    }

    #region Pointer

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left)
            UIManager.Instance.Push(new TetrisItemPanelContext(TetrisItemPanel.uiType, Input.mousePosition,this));
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
        InventoryDrager.StartDrag(inventoryTetris,this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        InventoryDrager.EndDrag(inventoryTetris);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
    #endregion

    #region 视觉效果
    public void SetBlockColor(DragState state)
    {
        var color = blockColorSetting.Find((a) => a.key == state).value;
        block.color = color;
    }

    private float _imgCellSize = 0;
    private void adjustVisualSize(float size)
    {
        _imgCellSize = size;
        RectTransform rect = transform as RectTransform;
        UIExtend.SetSize(rect, new Vector2(itemInfo.width, itemInfo.height) * _imgCellSize);

        img.sprite = itemInfo.icon;
        float cellWidth = itemInfo.width * _imgCellSize;
        float cellHeight = itemInfo.height * _imgCellSize;
        UIExtend.FitImgSize(img, cellWidth, cellHeight);

        onUseTip.transform.localPosition = new Vector2(cellWidth, cellHeight) / 2;
        numTip.transform.localPosition = new Vector2(cellWidth, -cellHeight) / 2;
    }

    #endregion

    public enum DragState
    {
        Placed,Placeable,Blocked,Stackable, Compositable
    }
}
