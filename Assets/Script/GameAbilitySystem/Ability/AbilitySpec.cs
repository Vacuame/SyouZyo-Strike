using BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class AbilitySpec
{
    protected object[] _abilityArguments;
    public AbilitySpec(AbstractAbility ability, AbilitySystemComponent owner)
    {
        this.ability = ability;
        this.owner = owner;
    }
    public AbstractAbility ability { get; }
    public AbilitySystemComponent owner { get; protected set; }
    public bool IsActive { get; private set; }
    public virtual bool CanActivate()
    {
        return !IsActive
                && CheckOtherCondition()
               && CheckGameplayTagsValid();
               //&& CheckCost()
               //&& CheckCooldown().TimeRemaining <= 0;
    }
    protected bool CheckGameplayTagsValid()
    {
        bool hasAllRequires = owner.HasAllTags(ability.Tag.ActivationRequiredTags);//有条件tag
        bool notBlockedByCharacter = !owner.HasAnyTags(ability.Tag.ActivationBlockedTags);//没有被tag阻挡

        bool notBlockedByOtherAbility = true;
        foreach (var kv in owner.AbilityContainer.AbilitySpecs)
        {
            var abilitySpec = kv.Value;
            if (abilitySpec.IsActive)
                if (ability.Tag.AssetTag.HasAnyTags(abilitySpec.ability.Tag.BlockAbilitiesWithTags))
                {
                    notBlockedByOtherAbility = false;
                    break;
                }
        }
        return hasAllRequires && notBlockedByCharacter && notBlockedByOtherAbility;
    }
    protected virtual bool CheckOtherCondition()//额外条件，由子类写
    {
        return true;
    }
    public void Tick()
    {
        SustainedTick();
        if (IsActive)
            AbilityTick();
    }
    public virtual bool TryActivateAbility(params object[] args)
    {
        _abilityArguments = args;
        if (!CanActivate()) return false;
        IsActive = true;

        owner.GameplayTagAggregator.ApplyAbilityTags(this);
        ActivateAbility(_abilityArguments);
        return true;
    }
    /// <summary>
    /// 当被其他动作打断
    /// </summary>
    public virtual void TryCancelAbility(AbilitySpec cancelBy)
    {
        if (!IsActive) return;
        IsActive = false;

        owner.GameplayTagAggregator.RestoreAbilityTags(this);
        CancelAbility(cancelBy);
    }
    /// <summary>
    /// 当主动结束
    /// </summary>
    public virtual void TryEndAbility()
    {
        if (!IsActive) return;
        IsActive = false;

        owner.GameplayTagAggregator.RestoreAbilityTags(this);
        EndAbility();
    }

    protected void EndSelf()
    {
        owner.TryEndAbility(ability.Name);
    }

    public virtual void OnGet() { }
    public virtual void OnRemoved() { }

    #region 被Try调用的抽象方法
    public virtual void AnimatorMove() { }
    protected virtual void AbilityTick(){}
    protected virtual void SustainedTick() { }//TODO 这是Buff系统的临时代替者，写了Buff系统就不要它了

    //默认被取消和主动结束是一个效果
    public virtual void CancelAbility(AbilitySpec cancelBy) => EndAbility();
    public abstract void ActivateAbility(params object[] args);
    public abstract void EndAbility();
    #endregion
}
