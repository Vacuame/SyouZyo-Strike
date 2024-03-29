using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayCueSpec
{
    protected readonly GameplayCue _cue;
    protected readonly GameplayCueParameters parameters;
    public AbilitySystemComponent Owner { get; protected set; }

    public virtual bool Triggerable()
    {
        return _cue.Triggerable(Owner);
    }

    public GameplayCueSpec(GameplayCue cue, GameplayCueParameters cueParameters)
    {
        this._cue = cue;
        parameters = cueParameters;
        if (parameters.sourceGameplayEffectSpec != null)
            Owner = parameters.sourceGameplayEffectSpec.Owner;
        else if (parameters.sourceAbilitySpec != null)
            Owner = parameters.sourceAbilitySpec.owner;
    }
}
