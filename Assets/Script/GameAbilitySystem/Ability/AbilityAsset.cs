using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityAsset : ScriptableObject
{
    [Header("ª˘±æ…Ë÷√")]
    public string abilityName;

    public string[] assetTags;
    public string[] cancelAbilityTags;
    public string[] blockAbilityTags;
    public string[] activationOwnedTag;
    public string[] activationRequiredTags;
    public string[] activationBlockedTags;
}
