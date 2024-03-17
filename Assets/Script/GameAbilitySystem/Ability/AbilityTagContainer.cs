using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityTagContainer
{
    public GameplayTagSet AssetTag;

    public GameplayTagSet CancelAbilitiesWithTags;//能力启用时，会取消这些Tag的能力
    public GameplayTagSet BlockAbilitiesWithTags;

    //暂时不用
    public GameplayTagSet ActivationOwnedTag;
    public GameplayTagSet ActivationRequiredTags;
    public GameplayTagSet ActivationBlockedTags;

    public AbilityTagContainer(
        GameplayTag[] assetTags,
        GameplayTag[] cancelAbilityTags,
        GameplayTag[] blockAbilityTags,
        GameplayTag[] activationOwnedTag,
        GameplayTag[] activationRequiredTags,
        GameplayTag[] activationBlockedTags)
    {
        AssetTag = new GameplayTagSet(assetTags);
        CancelAbilitiesWithTags = new GameplayTagSet(cancelAbilityTags);
        BlockAbilitiesWithTags = new GameplayTagSet(blockAbilityTags);
        ActivationOwnedTag = new GameplayTagSet(activationOwnedTag);
        ActivationRequiredTags = new GameplayTagSet(activationRequiredTags);
        ActivationBlockedTags = new GameplayTagSet(activationBlockedTags);
    }

}