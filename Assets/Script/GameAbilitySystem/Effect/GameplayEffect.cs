using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EffectsDurationPolicy
{
    None,
    Instant,
    Infinite,
    Duration
}
public abstract class GameplayEffect
{
    public GameplayEffectAsset asset;

    public readonly float period;
    public readonly float duration;
    public EffectsDurationPolicy durationPolicy;
    public GameplayEffectTagContainer TagContainer;

    public GameplayEffect(GameplayEffectAsset ass)
    {
        asset = ass;
        period = asset.period;
        duration = asset.duration;
        durationPolicy = asset.durationPolicy;
        TagContainer = new GameplayEffectTagContainer(
            asset.assetTags,
            asset.applicationRequiredTags,
            asset.removeGameplayEffectsWithTags,
            asset.applicationImmunityTags
            );
    }

    public abstract GameplayEffectSpec CreateSpec(AbilitySystemComponent owner,AbilitySystemComponent source);

}

public abstract class GameplayEffect<T> : GameplayEffect where T : GameplayEffectAsset
{
    protected readonly T EffectAsset;
    public GameplayEffect(GameplayEffectAsset ass) : base(ass)
    {
        EffectAsset = ass as T;
    }

}
