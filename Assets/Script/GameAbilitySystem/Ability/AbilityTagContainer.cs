using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityTagContainer
{
    public GameplayTagSet AssetTag;//能力本身的tag

    public GameplayTagSet CancelAbilitiesWithTags;//如果我运行会取消哪些能力
    public GameplayTagSet BlockAbilitiesWithTags;//我会阻塞哪些能力（让他们不能启动）

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