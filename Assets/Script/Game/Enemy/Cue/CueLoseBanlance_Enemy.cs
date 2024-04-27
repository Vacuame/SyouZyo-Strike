using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理LoseBanlance的实际逻辑
/// </summary>
[CreateAssetMenu(fileName = "NewData", menuName = "ABS/GameplayEffect/Cue/LoseBanlance")]
public class CueLoseBanlance_Enemy : GameplayCueInstant
{
    [Serializable]
    public struct PartBalanceSet
    {
        public float loseBanlanceDuration;
        public string animName;
    }

    [SerializeField] private PartBalanceSet defaultPartSet;

    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueLoseBanlanceSpec(this, parameters);
    }

    public class CueLoseBanlanceSpec : GameplayCueInstantSpec
    {
        CueLoseBanlance_Enemy loseBanlance;
        public CueLoseBanlanceSpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue, parameters)
        {
            loseBanlance = cue as CueLoseBanlance_Enemy;
        }

        public override void Trigger()
        {
            parameters.sourceGameplayEffectSpec.duration = loseBanlance.defaultPartSet.loseBanlanceDuration;

            Owner.GetComponent<Character>().anim.Play(loseBanlance.defaultPartSet.animName);
        }
    }

}
