using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO 应该改名叫EquipedItemInfo
[CreateAssetMenu(fileName = "NewEquip", menuName = "Item/EquipedInfo")]
public class EquipedItemInfo : ItemInfo
{
    public EquipedItem equipedItemPrefab;
}
