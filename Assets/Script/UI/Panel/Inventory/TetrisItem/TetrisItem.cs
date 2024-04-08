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

    [SerializeField] private Image block;
    [SerializeField] private List<Pair<DragState, Color>> blockColorSetting;

    public GameObject onUseTip;

    private CanvasGroup canvasGroup;
    public InventoryTetris inventoryTetris { get; private set; }
    private InventoryDrager InventoryDrager => inventoryTetris.inventoryPanel.inventoryDrager;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetInfo(ItemInfo itemInfo, ItemSave itemSave)
    {
        this.itemInfo = itemInfo;
        this.itemSave = itemSave;

        switch(itemInfo.type)
        {
            case ItemInfo.ItemType.Gun:
                GunItemSave gunSave = itemSave as GunItemSave;
                onUseTip.SetActive(gunSave.equiped);
                gunSave.onEquipedChange += OnEquipedChange;
                break;
        }
    }

    public void OnEquipedChange(bool eq)
    {
        if (onUseTip!=null)
        {
            onUseTip.SetActive(eq);
        }
        
    }

    private void OnDestroy()
    {
        switch (itemInfo.type)
        {
            case ItemInfo.ItemType.Gun:
                GunItemSave gunSave = itemSave as GunItemSave;
                gunSave.onEquipedChange -= OnEquipedChange;
                break;
        }
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return InventoryStatic.GetGridPositionList(gridPos, dir,itemInfo);
    }

    #region Pointer

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left)
        UIManager.Instance.Push(new TetrisItemPanelContext
            (TetrisItemPanel.uiType, Input.mousePosition,this));
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

    public void SetBlockColor(DragState state)
    {
        var color = blockColorSetting.Find((a) => a.key == state).value;
        block.color = color;
    }

    public void SetTetris(Vector2Int gridPos, Dir dir, InventoryTetris inventoryTetris)
    {
        this.gridPos = gridPos;
        this.dir = dir;
        this.inventoryTetris = inventoryTetris;

        transform.SetParent(inventoryTetris.GetItemContainer());
        transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(dir));
        Grid<ItemBlock>grid = inventoryTetris.GetGrid();
        Vector3 anchoredPostion =grid.GetTransformPosition(gridPos.x, gridPos.y) +
                       (Vector3)GetRotationOffset(itemInfo,dir) * grid.GetCellSize() / 2;
        transform.localPosition = anchoredPostion;
    }

    public enum DragState
    {
        Placed,Placeable,Blocked
    }
}
