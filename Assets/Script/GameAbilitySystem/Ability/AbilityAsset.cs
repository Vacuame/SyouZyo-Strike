using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ABS/Ability/Basic")]
public class AbilityAsset : ScriptableObject
{
    [Header("基本设置")]
    public string abilityName;//TODO 这个没用了，处理一下，到底Ability的名字是什么？

    public string[] assetTags;
    public string[] cancelAbilityTags;
    public string[] blockAbilityTags;
    public string[] activationOwnedTag;
    public string[] activationRequiredTags;
    public string[] activationBlockedTags;
}
