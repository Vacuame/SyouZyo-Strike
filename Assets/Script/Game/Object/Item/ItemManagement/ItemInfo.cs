using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public int id;
    public string itemName;
    public int width;
    public int height;
    public Transform tetrisItemPrefab;

}
