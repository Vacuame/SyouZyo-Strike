using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static InventoryItem_SO;

[RequireComponent(typeof(CanvasGroup))]
public class TetrisItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public InventoryItem_SO item_SO;
    [HideInInspector]public Dir dir;
    [HideInInspector] public Vector2Int gridPos;
    private CanvasGroup canvasGroup;
    private InventoryTetris inventoryTetris;
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
    }

    #endregion

    public static TetrisItem Instantiate(InventoryItem_SO itemSO, Transform itemContainer, Vector2 anchoredPosition, Vector2Int gridPos, Dir dir, InventoryTetris tetris)
    {
        Transform placedObjectTransform = Instantiate(itemSO.prefab, itemContainer);
        placedObjectTransform.rotation = Quaternion.Euler(0, itemSO.GetRotationAngle(dir), 0);
        placedObjectTransform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        TetrisItem placedObject = placedObjectTransform.GetComponent<TetrisItem>();
        placedObject.item_SO = itemSO;
        placedObject.gridPos = gridPos;
        placedObject.dir = dir;
        placedObject.inventoryTetris = tetris;

        return placedObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
