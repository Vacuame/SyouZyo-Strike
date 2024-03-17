using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҫ������ض����趨�����ݣ�������������Spec
/// </summary>
public abstract class AbstractAbility
{
    public readonly string Name;

    public AbstractAbility(AbilityAsset Asset)
    {
       Tag = new AbilityTagContainer(Asset.assetTags, Asset.cancelAbilityTags,
           Asset.blockAbilityTags, Asset.activationOwnedTag,
           Asset.activationRequiredTags,Asset.blockAbilityTags);
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