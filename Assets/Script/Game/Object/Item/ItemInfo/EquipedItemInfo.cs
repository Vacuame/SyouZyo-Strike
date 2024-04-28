using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO 应该改名叫EquipedItemInfo
[CreateAssetMenu(fileName = "NewEquip", menuName = "Item/EquipedInfo")]
public class EquipedItemInfo : ItemInfo
{
    [Header("装备设定")]
    public EquipedItem equipedItemPrefab;
    public Sprite simpleIcon;
}
