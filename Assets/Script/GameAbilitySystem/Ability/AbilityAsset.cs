using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityAsset : ScriptableObject
{
    public GameplayTag[] assetTags;
    public GameplayTag[] cancelAbilityTags;
    public GameplayTag[] blockAbilityTags;
    public GameplayTag[] activationOwnedTag;
    public GameplayTag[] activationRequiredTags;
    public GameplayTag[] activationBlockedTags;
}
