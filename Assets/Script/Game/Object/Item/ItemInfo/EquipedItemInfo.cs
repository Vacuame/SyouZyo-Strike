using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Ӧ�ø�����EquipedItemInfo
[CreateAssetMenu(fileName = "NewEquip", menuName = "Item/EquipedInfo")]
public class EquipedItemInfo : ItemInfo
{
    [Header("װ���趨")]
    public EquipedItem equipedItemPrefab;
    public Sprite simpleIcon;
}
