using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ABS/Ability/Basic")]
public class AbilityAsset : ScriptableObject
{
    [Header("��������")]
    public string abilityName;

    public string[] assetTags;
    public string[] cancelAbilityTags;
    public string[] blockAbilityTags;
    public string[] activationOwnedTag;
    public string[] activationRequiredTags;
    public string[] activationBlockedTags;
}
