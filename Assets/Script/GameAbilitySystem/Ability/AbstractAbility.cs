using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主要负责加载动作设定的数据，根据数据生成Spec
/// </summary>
public abstract class AbstractAbility
{
    public readonly string Name;
    public AbstractAbility(AbilityAsset setAsset)
    {
        Name = setAsset.abilityName != ""? setAsset.abilityName: GetType().Name;

        Tag = new AbilityTagContainer(setAsset.assetTags, setAsset.cancelAbilityTags,
           setAsset.blockAbilityTags, setAsset.activationOwnedTag,
           setAsset.activationRequiredTags,setAsset.activationBlockedTags);
    }

    public AbilityTagContainer Tag;
    public abstract AbilitySpec CreateSpec(AbilitySystemComponent owner);

}

public abstract class AbstractAbility<T> : AbstractAbility where T : AbilityAsset
{
    protected readonly T AbilityAsset;
    protected AbstractAbility(AbilityAsset abilityAsset) : base(abilityAsset)
    {
        AbilityAsset = abilityAsset as T;
    }
}