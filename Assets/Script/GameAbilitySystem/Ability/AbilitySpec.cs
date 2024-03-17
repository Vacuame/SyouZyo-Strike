using BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class AbilitySpec
{
    protected object[] _abilityArguments;
    public AbilitySpec(AbstractAbility ability, AbilitySystemComponent owner)
    {
        Ability = ability;
        Owner = owner;
    }
    public AbstractAbility Ability { get; }
    public AbilitySystemComponent Owner { get; protected set; }
    public bool IsActive { get; private set; }
    public virtual bool CanActivate()
    {
        return !IsActive
               && CheckGameplayTagsValidTpActivate();
               //&& CheckCost()
               //&& CheckCooldown().TimeRemaining <= 0;
    }
    private bool CheckGameplayTagsValidTpActivate()
    {
        var hasAllTags = Owner.HasAllTags(Ability.Tag.ActivationRequiredTags);
        var notHasAnyTags = !Owner.HasAnyTags(Ability.Tag.ActivationBlockedTags);
        var notBlockedByOtherAbility = true;

        foreach (var kv in Owner.AbilityContainer.AbilitySpecs)
        {
            var abilitySpec = kv.Value;
            if (abilitySpec.IsActive)
                if (Ability.Tag.AssetTag.HasAnyTags(abilitySpec.Ability.Tag.BlockAbilitiesWithTags))
                {
                    notBlockedByOtherAbility = false;
                    break;
                }
        }
        return hasAllTags && notHasAnyTags && notBlockedByOtherAbility;
    }
    public void Tick()
    {
        if (!IsActive) return;
        AbilityTick();
    }
    public virtual bool TryActivateAbility(params object[] args)
    {
        _abilityArguments = args;
        if (!CanActivate()) return false;
        IsActive = true;

        //Owner.GameplayTagAggregator.ApplyGameplayAbilityDynamicTag(this);
        ActivateAbility(_abilityArguments);
        return true;
    }
    public virtual void TryCancelAbility()
    {
        if (!IsActive) return;
        IsActive = false;

        //Owner.GameplayTagAggregator.RestoreGameplayAbilityDynamicTags(this);
        CancelAbility();
    }
    public virtual void TryEndAbility()
    {
        if (!IsActive) return;
        IsActive = false;

        //Owner.GameplayTagAggregator.RestoreGameplayAbilityDynamicTags(this);
        EndAbility();
    }

    protected virtual void AbilityTick(){}
    public abstract void CancelAbility();
    public abstract void ActivateAbility(params object[] args);
    public abstract void EndAbility();

}
