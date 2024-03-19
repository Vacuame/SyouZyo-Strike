using System.Collections;
using UnityEngine;


public class GameplayEffectAsset : ScriptableObject
{
    public float period;
    public float duration;
    public EffectsDurationPolicy durationPolicy;

    public string[] assetTags;
    public string[] applicationRequiredTags;
    public string[] removeGameplayEffectsWithTags;
    public string[] applicationImmunityTags;
}
