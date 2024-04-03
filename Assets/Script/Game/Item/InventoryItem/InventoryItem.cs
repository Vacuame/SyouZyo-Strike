using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public InventoryItem_SO item_SO;
    public InventoryItem_SO.Dir dir;
    public Vector2Int gridPos;



    public static InventoryItem Instantiate(Transform parent, Vector2 anchoredPosition, Vector2Int pos, InventoryItem_SO.Dir dir, InventoryItem_SO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, parent);
        placedObjectTransform.rotation = Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        placedObjectTransform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        InventoryItem placedObject = placedObjectTransform.GetComponent<InventoryItem>();
        placedObject.item_SO = placedObjectTypeSO;
        placedObject.gridPos = pos;
        placedObject.dir = dir;

        return placedObject;
    }
}
