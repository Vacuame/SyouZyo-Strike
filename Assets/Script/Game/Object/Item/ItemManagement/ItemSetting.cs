using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemSetting", menuName = "Item/ItemInfoList")]
public class ItemInfoSetting : ScriptableObject
{
    public MultiSetting<int, ItemConfig> setting;

    [System.Serializable]
    public struct ItemConfig
    {
        public ItemInfo itemInfo;
        public PickableObj pickablePrefab;
    }
}
