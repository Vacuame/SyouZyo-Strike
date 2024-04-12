using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// 处理LoseBanlance的实际逻辑
/// </summary>
[CreateAssetMenu(fileName = "NewData", menuName = "ABS/GameplayEffect/Cue/EndAbility")]
public class CueEndAbility : GameplayCueInstant
{
    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueEndAbilitySpec(this, parameters);
    }

    public bool endAll;

    public string[] endNames;
    public string[] endTags;
    public string[] exceptEndTags;

    public class CueEndAbilitySpec : GameplayCueInstantSpec
    {

        CueEndAbility cueEndAbility;

        public CueEndAbilitySpec(GameplayCueInstant cue, GameplayCueParameters parameters) : base(cue, parameters)
        {
            cueEndAbility = cue as CueEndAbility;
            endTagSet = new GameplayTagSet(cueEndAbility.endTags);
            exceptEndTagSet = new GameplayTagSet(cueEndAbility.exceptEndTags);
        }

        GameplayTagSet endTagSet;
        GameplayTagSet exceptEndTagSet;

        public override void Trigger()
        {
            if (cueEndAbility.endAll)
            {
                foreach (string a in Owner.AbilityContainer.AbilitySpecs.Keys)
                    Owner.TryEndAbility(a);
            }
            else
            {
                foreach (string a in cueEndAbility.endNames)
                    Owner.TryEndAbility(a);

                foreach(var a in Owner.AbilityContainer.AbilitySpecs)
                {
                    var abilityTag = a.Value.ability.Tag;
                    if (abilityTag.AssetTag.HasAnyTags(endTagSet))
                    {
                        if (abilityTag.AssetTag.HasNoneTags(exceptEndTagSet))
                            Owner.TryEndAbility(a.Value.ability.Name);
                    }
                }
            }
        }
    }

}
