using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityTagContainer
{
    public GameplayTagSet AssetTag;

    public GameplayTagSet CancelAbilitiesWithTags;//��������ʱ����ȡ����ЩTag������
    public GameplayTagSet BlockAbilitiesWithTags;

    //��ʱ����
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