using MoleMole;
using System.Collections.Generic;
using UnityEngine;
using static TetrisItem_SO;
using static TetrisItem;

public class InventoryDrager
{
    private List<InventoryTetris> inventoryTetrisList;

    //Drag
    private InventoryTetris draggingTetris;
    private Vector2Int selectedGridPos;
    private TetrisItem draggingItem;
    private Dir dragDir;
    public Vector3 mouseGridOffset;
    private DragState dragState;

    public Dir GetDir() { return dragDir; }

    //Rotate
    private float curRotation;
    private float rotateSpeed;
    private const float rotateTime = 0.3f;

    public InventoryDrager(List<InventoryTetris> list) 
    {
        inventoryTetrisList = list;
        rotateSpeed = 90 /rotateTime * Time.deltaTime;
    }

    public void Tick()
    {
        if(draggingItem != null) 
        {
            //计算位置
            Vector3 screenPos = Input.mousePosition + mouseGridOffset;

            if (GetTetrisInScreen(screenPos, out InventoryTetris tetris))
                draggingTetris = tetris;
            else
                return;

            Grid<ItemBlock> grid = draggingTetris.GetGrid();
            selectedGridPos = draggingTetris.GetGridPosInScreen(screenPos);
            
            //预览放置效果
            Vector3 rectPostion =grid.GetTransformPosition(selectedGridPos.x, selectedGridPos.y) +
                   ((Vector3)draggingItem.item_SO.GetRotationOffset(dragDir)+Vector3.one * 0.5f) * grid.GetCellSize() / 2;
            draggingItem.transform.localPosition = rectPostion;

            if (draggingTetris.CanDragTo(draggingItem, selectedGridPos, dragDir))
                dragState = DragState.Placeable;
            else
                dragState = DragState.Blocked;
            draggingItem.SetBlockColor(dragState);

            //旋转
            if (Input.GetKeyDown(KeyCode.Q))
                dragDir = GetNextDir(dragDir);
            float targetRotation = GetRotationAngle(dragDir);
            curRotation = Mathf.MoveTowardsAngle(curRotation, targetRotation, rotateSpeed);
            draggingItem.transform.rotation = Quaternion.Euler(0, 0, curRotation);
        }
    }

    public void StartDrag(InventoryTetris inventoryTetris, TetrisItem item)
    {
        draggingTetris = inventoryTetris;
        draggingItem = item;
        dragDir = draggingItem.dir;
        curRotation = draggingItem.transform.rotation.eulerAngles.z;

        //计算Mouse Offset，比如点了图片中间，我实际指向应该是图片左下角，因为放置是从左下角开始的
        Vector3 screenPoint = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (draggingTetris.GetItemContainer(), screenPoint, null, out Vector2 anchoredPos);

        Vector3 itemLeftDownPos =item.transform.localPosition - 
               (Vector3)item.item_SO.GetRotationOffset(dragDir) * draggingTetris.GetGrid().GetCellSize() / 2;

        mouseGridOffset = itemLeftDownPos - (Vector3)anchoredPos;
    }

    public void EndDrag(InventoryTetris fromInventoryTetris)
    {
        if(dragState == DragState.Placeable)
        {
            fromInventoryTetris.ClearGridItem(draggingItem);
            draggingTetris.SetGridItem(draggingItem, selectedGridPos, dragDir);
            draggingItem.SetTetris(selectedGridPos, dragDir, draggingTetris);
        }
        else//返回原位
        {
            draggingItem.SetTetris(draggingItem.gridPos, draggingItem.dir, draggingItem.inventoryTetris);
        }
        draggingItem.SetBlockColor(DragState.Placed);
        draggingItem = null;
        draggingTetris = null;
    }

    private bool GetTetrisInScreen(Vector3 screenPos,out InventoryTetris res)
    {
        res = null;
        foreach(var a in inventoryTetrisList)
        {
            Vector2Int gridPos = a.GetGridPosInScreen(screenPos);
            if (a.GetGrid().IsValidGridPosition(gridPos))
            {
                res = a;
                return true;
            }
        }
        return false;
    }
}
