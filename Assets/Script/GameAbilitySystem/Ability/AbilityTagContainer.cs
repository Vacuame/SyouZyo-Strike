using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityTagContainer
{
    public GameplayTagSet AssetTag;//���������tag

    public GameplayTagSet CancelAbilitiesWithTags;//��������ʱ����ȡ��������ЩTag������
    public GameplayTagSet BlockAbilitiesWithTags;//����֮��Ļ��⣬����������������Щtag�����Ҳ�������

    public GameplayTagSet ActivationOwnedTag;//��������ʱ����Щ�Ӹ���ɫ
    public GameplayTagSet ActivationRequiredTags;//��Ҫ��ɫ����Щ������������
    public GameplayTagSet ActivationBlockedTags;//�ᱻ��ɫ����Щ��ס����������

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