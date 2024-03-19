using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityTagContainer
{
    public GameplayTagSet AssetTag;//能力本身的tag

    public GameplayTagSet CancelAbilitiesWithTags;//能力启用时，会取消包含这些Tag的能力
    public GameplayTagSet BlockAbilitiesWithTags;//能力之间的互斥，有启用能力包含这些tag，则我不能启用

    public GameplayTagSet ActivationOwnedTag;//能力启动时把这些加给角色
    public GameplayTagSet ActivationRequiredTags;//需要角色有这些才能启用能力
    public GameplayTagSet ActivationBlockedTags;//会被角色的这些挡住，不能启用

    public AbilityTagContainer(
         string[] assetTags,
         string[] cancelAbilityTags,
         string[] blockAbilityTags,
         string[] activationOwnedTag,
         string[] activationRequiredTags,
         string[] activationBlockedTags)
    {
        AssetTag = new GameplayTagSet(assetTags);
        CancelAbilitiesWithTags = new GameplayTagSet(cancelAbilityTags);
        BlockAbilitiesWithTags = new GameplayTagSet(blockAbilityTags);
        ActivationOwnedTag = new GameplayTagSet(activationOwnedTag);
        ActivationRequiredTags = new GameplayTagSet(activationRequiredTags);
        ActivationBlockedTags = new GameplayTagSet(activationBlockedTags);
    }

}