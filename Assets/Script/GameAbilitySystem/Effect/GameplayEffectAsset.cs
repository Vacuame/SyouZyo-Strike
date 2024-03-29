using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameplayEffect", menuName = "ABS/GameplayEffect/GameplayEffect")]
public class GameplayEffectAsset : ScriptableObject
{
    public float period;
    public float duration;
    public EffectsDurationPolicy durationPolicy;

    public string[] assetTags;
    public string[] applicationRequiredTags;
    public string[] removeGameplayEffectsWithTags;
    public string[] applicationImmunityTags;

    public GameplayCueInstant[] CueOnExecute;
    public GameplayCueInstant[] CueOnRemove;
    public GameplayCueInstant[] CueOnAdd;
    public GameplayCueDurational[] CueDurational;
    public GameplayEffectModifier[] Modifiers;
}
