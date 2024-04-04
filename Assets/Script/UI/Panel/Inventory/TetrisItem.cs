using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryStatic;
using static InventoryTetris;

[RequireComponent(typeof(CanvasGroup))]
public class TetrisItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ItemInfo itemSO;
    [HideInInspector] public Dir dir;
    [HideInInspector] public Vector2Int gridPos;
    [SerializeField] private Image block;
    [SerializeField] private List<Pair<DragState, Color>> blockColorSetting;
    private CanvasGroup canvasGroup;
    public InventoryTetris inventoryTetris { get; private set; }
    private InventoryDrager InventoryDrager => inventoryTetris.inventoryPanel.inventoryDrager;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return InventoryStatic.GetGridPositionList(gridPos, dir,itemSO);
    }

    #region Control
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
                       (Vector3)GetRotationOffset(itemSO,dir) * grid.GetCellSize() / 2;
        transform.localPosition = anchoredPostion;
    }

    public static TetrisItem Instantiate(ItemInfo itemSO, Transform itemContainer, Vector2 anchoredPosition, Vector2Int gridPos, Dir dir, InventoryTetris tetris)
    {
        Transform placedObjectTransform = Instantiate(itemSO.tetrisItemPrefab);
        TetrisItem placedObject = placedObjectTransform.GetComponent<TetrisItem>();
        placedObject.itemSO = itemSO;
        placedObject.SetTetris(gridPos, dir, tetris);

        return placedObject;
    }

    
    public enum DragState
    {
        Placed,Placeable,Blocked
    }
}
