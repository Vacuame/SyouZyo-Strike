using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;
using MoleMole;
using System.Collections.Generic;
using UnityEngine;
using static InventoryItem_SO;
using static UnityEditor.Progress;

public class InventoryDrager
{
    private List<InventoryTetris> inventoryTetrisList;
    private InventoryTetris draggingTetris;
    private TetrisItem draggingItem;
    private Dir dir;
    public Vector3 mouseGridOffset;
    private Vector3 mouseItemOffset;

    public InventoryDrager(List<InventoryTetris> list) 
    {
        inventoryTetrisList = list;
    }

    public void Tick()
    {
        if(draggingItem != null) 
        {
            Vector3 ItemPos = Input.mousePosition + mouseItemOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (draggingTetris.GetItemContainer(), ItemPos, null, out Vector2 anchoredItemPos);
            draggingItem.transform.localPosition = anchoredItemPos;
        }
    }

    public void StartDrag(InventoryTetris inventoryTetris, TetrisItem item)
    {
        draggingTetris = inventoryTetris;
        draggingItem = item;
        dir = draggingItem.dir;

        //����Mouse Offset���������ͼƬ�м䣬��ʵ��ָ��Ӧ����ͼƬ���½ǣ���Ϊ�����Ǵ����½ǿ�ʼ��
        Vector3 screenPoint = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (draggingTetris.GetItemContainer(), screenPoint, null, out Vector2 anchoredPos);
        Vector3 itemLeftDownPos =item.transform.localPosition - 
               (Vector3)item.item_SO.GetRotationOffset(dir) * draggingTetris.GetGrid().GetCellSize() / 2;

        mouseItemOffset = item.transform.localPosition - (Vector3)anchoredPos;
        mouseGridOffset = itemLeftDownPos - (Vector3)anchoredPos;
        Debug.Log(mouseItemOffset); 
        Debug.Log(mouseGridOffset);

        Cursor.visible = false;
    }

    public void EndDrag() 
    {
        Cursor.visible = true;
    }
}
