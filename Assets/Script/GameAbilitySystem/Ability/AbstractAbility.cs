using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҫ������ض����趨�����ݣ�������������Spec
/// </summary>
public abstract class AbstractAbility
{
    public readonly string Name;
    public AbstractAbility(AbilityAsset setAsset, params object[] binds)
    {
        Name = setAsset.abilityName != ""? setAsset.abilityName: GetType().Name;

        this.binds = binds;

        Tag = new AbilityTagContainer(setAsset.assetTags, setAsset.cancelAbilityTags,
           setAsset.blockAbilityTags, setAsset.activationOwnedTag,
           setAsset.activationRequiredTags,setAsset.activationBlockedTags);
    }

    public object[] binds;

    public AbilityTagContainer Tag;
    public abstract AbilitySpec CreateSpec(AbilitySystemComponent owner);

}

public abstract class AbstractAbility<T> : AbstractAbility where T : AbilityAsset
{
    protected readonly T AbilityAsset;
    protected AbstractAbility(AbilityAsset abilityAsset, params object[] binds) : base(abilityAsset,binds)
    {
        AbilityAsset = abilityAsset as T;
    }
}