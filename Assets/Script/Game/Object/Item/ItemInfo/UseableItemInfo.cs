using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUseableItem", menuName = "Item/UseableItemInfo")]
public class UseableItemInfo : ItemInfo
{
    [Header("����Ч��")]
    public List<GameplayEffectAsset> effectAssets;
}
