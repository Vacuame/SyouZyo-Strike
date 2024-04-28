using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUseableItem", menuName = "Item/UseableItemInfo")]
public class UseableItemInfo : ItemInfo
{
    [Header("道具效果")]
    public List<GameplayEffectAsset> effectAssets;
}
