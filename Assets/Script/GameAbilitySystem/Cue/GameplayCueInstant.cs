using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InstantCueApplyTarget
{
    Owner,
    Source
}

//¼´Ê±´¥·¢µÄCue
public abstract class GameplayCueInstant : GameplayCue<GameplayCueInstantSpec>
{
    public InstantCueApplyTarget applyTarget;

    public virtual void ApplyFrom(GameplayEffectSpec gameplayEffectSpec)
    {
        if (Triggerable(gameplayEffectSpec.Owner))
        {
            var instantCue = CreateSpec(new GameplayCueParameters
            { sourceGameplayEffectSpec = gameplayEffectSpec });

            instantCue?.Trigger();
        }
    }

    public virtual void ApplyFrom(AbilitySpec abilitySpec, params object[] customArguments)
    {
        if (Triggerable(abilitySpec.owner))
        {
            var instantCue = CreateSpec(new GameplayCueParameters
            { sourceAbilitySpec = abilitySpec, 
                customArguments = customArguments });

            instantCue?.Trigger();
        }
    }

}

public abstract class GameplayCueInstantSpec : GameplayCueSpec
{
    public GameplayCueInstantSpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue,
        parameters)
    {

    }

    public abstract void Trigger();
}

public abstract class GameplayCueInstantSpec<T> : GameplayCueInstantSpec where T : GameplayCueInstant
{
    public readonly T cue;

    public GameplayCueInstantSpec(T cue, GameplayCueParameters parameters) : base(cue, parameters)
    {
        this.cue = cue;
    }
}