using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "ABS/GameplayEffect/Cue/PlayAnim")]
public class CuePlayAnim : GameplayCueInstant
{
    public AnimPlayConfig animConfig;
    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CuePlayAnimSpec(this, parameters);
    }

    public class CuePlayAnimSpec : GameplayCueInstantSpec
    {
        private readonly CuePlayAnim cuePlayAnim;

        public CuePlayAnimSpec(CuePlayAnim cue, GameplayCueParameters parameters) :
            base(cue, parameters)
        {
            cuePlayAnim = _cue as CuePlayAnim;
        }

        public override void Trigger()
        {
            var character = Owner.GetComponent<Character>();

            cuePlayAnim.animConfig.PlayAnim(character.anim);
        }
    }
}
