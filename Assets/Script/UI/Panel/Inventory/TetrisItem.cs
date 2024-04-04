using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static TetrisItem_SO;
using static UnityEditor.Progress;

[RequireComponent(typeof(CanvasGroup))]
public class TetrisItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public TetrisItem_SO item_SO;
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
        return item_SO.GetGridPositionList(gridPos, dir);
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

        transform.parent = inventoryTetris.GetItemContainer();
        transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(dir));
        Grid<ItemBlock>grid = inventoryTetris.GetGrid();
        Vector3 anchoredPostion =grid.GetTransformPosition(gridPos.x, gridPos.y) +
                       (Vector3)item_SO.GetRotationOffset(dir) * grid.GetCellSize() / 2;
        transform.localPosition = anchoredPostion;
    }

    public static TetrisItem Instantiate(TetrisItem_SO itemSO, Transform itemContainer, Vector2 anchoredPosition, Vector2Int gridPos, Dir dir, InventoryTetris tetris)
    {
        Transform placedObjectTransform = Instantiate(itemSO.prefab);
        //placedObjectTransform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        TetrisItem placedObject = placedObjectTransform.GetComponent<TetrisItem>();
        placedObject.item_SO = itemSO;
        placedObject.SetTetris(gridPos, dir, tetris);

        return placedObject;
    }

    
    public enum DragState
    {
        Placed,Placeable,Blocked
    }
}
