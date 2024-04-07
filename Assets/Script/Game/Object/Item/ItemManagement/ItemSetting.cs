using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemSetting", menuName = "Item/ItemInfoList")]
public class ItemInfoSetting : ScriptableObject
{
    public MultiSetting<int, ItemInfo> setting;
}
