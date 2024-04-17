using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public enum EffectsDurationPolicy
{
    None,
    Instant,
    Infinite,
    Duration
}
/// <summary>
/// 这个结构的作用只是做初步的检测,然后把Spec创建出来
/// </summary>
public struct GameplayEffect
{
    public GameplayEffectAsset asset;

    public readonly float period;
    public readonly float duration;
    public EffectsDurationPolicy durationPolicy;
    public GameplayEffectTagContainer TagContainer;

    // Cues
    public readonly GameplayCueInstant[] CueOnExecute;
    public readonly GameplayCueInstant[] CueOnRemove;
    public readonly GameplayCueInstant[] CueOnAdd;
    public readonly GameplayCueDurational[] CueDurational;
    public readonly GameplayEffectModifier[] Modifiers;

    public GameplayEffect(GameplayEffectAsset asset)
    {
        this.asset = asset;
        period = this.asset.period;
        duration = this.asset.duration;
        durationPolicy = this.asset.durationPolicy;
        TagContainer = new GameplayEffectTagContainer(
            this.asset.assetTags,
            this.asset.grantedTags,
            this.asset.applicationRequiredTags,
            this.asset.removeGameplayEffectsWithTags,
            this.asset.applicationImmunityTags
            );
        CueOnExecute = this.asset.CueOnExecute;
        CueOnRemove = this.asset.CueOnRemove;
        CueOnAdd = this.asset.CueOnAdd;
        CueDurational = this.asset.CueDurational;
        Modifiers = this.asset.Modifiers;
    }

    public GameplayEffectSpec CreateSpec(AbilitySystemComponent creator,AbilitySystemComponent owner)
    {
        return new GameplayEffectSpec(this, creator, owner);
    }

    public bool HasRequiredTag(AbilitySystemComponent target)
    {
        return target.HasAllTags(TagContainer.ApplicationRequiredTags);
    }

    public bool NULL => durationPolicy == EffectsDurationPolicy.None;
}
