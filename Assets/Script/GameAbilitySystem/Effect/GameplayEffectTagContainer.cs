using System.Collections;
using UnityEngine;

public struct GameplayEffectTagContainer
{
    public GameplayTagSet AssetTags;
    public GameplayTagSet GrantedTags;
    public GameplayTagSet ApplicationRequiredTags;//需要这些tag才能执行
    public GameplayTagSet RemoveGameplayEffectsWithTags;//buff运行时，会移除这些buff
    public GameplayTagSet ApplicationImmunityTags; //会被这些tag免疫

    public GameplayEffectTagContainer(
        string[] assetTags,
        string[] grantedTags,
        string[] applicationRequiredTags,
        string[] removeGameplayEffectsWithTags,
        string[] applicationImmunityTags)
    {
        AssetTags = new GameplayTagSet(assetTags);
        GrantedTags = new GameplayTagSet(grantedTags);
        ApplicationRequiredTags = new GameplayTagSet(applicationRequiredTags);
        RemoveGameplayEffectsWithTags = new GameplayTagSet(removeGameplayEffectsWithTags);
        ApplicationImmunityTags = new GameplayTagSet(applicationImmunityTags);
    }
}