using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO 应该改名叫EquipedItemInfo
[CreateAssetMenu(fileName = "NewGun", menuName = "Item/GunInfo")]
public class GunItemInfo : ItemInfo
{
    public EquipedItem equipedItemPrefab;
}
