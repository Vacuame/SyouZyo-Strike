using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewApplyEffect", menuName = "ABS/GameplayEffect/Cue/ApplyEffect")]
public class CueApplyEffect : GameplayCueInstant
{
    public GameplayEffectAsset effectAsset;
    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueApplyEffectSpec(this, parameters);
    }

    public class CueApplyEffectSpec : GameplayCueInstantSpec
    {
        CueApplyEffect cueApplyEffect;
        public CueApplyEffectSpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue, parameters)
        {
            cueApplyEffect = cue as CueApplyEffect;
        }

        public override void Trigger()
        {
            Owner.ApplyGameplayEffectToSelf(new GameplayEffect(cueApplyEffect.effectAsset));
        }
    }
}
