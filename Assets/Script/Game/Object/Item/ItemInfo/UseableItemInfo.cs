using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUseableItem", menuName = "Item/UseableItemInfo")]
public class UseableItemInfo : ItemInfo
{
    public List<GameplayEffectAsset> effectAssets;
}
