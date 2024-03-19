using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.UI.GridLayoutGroup;


public class GameplayEffectSpec
{
    public EffectsDurationPolicy DurationPolicy => GameplayEffect.durationPolicy;
    public GameplayEffect GameplayEffect { get; }
    public GameplayEffectSpec PeriodExecution { get; private set; }
    public GameplayEffectPeriodTicker PeriodTicker { get; }
    public AbilitySystemComponent Source { get; }
    public AbilitySystemComponent Owner { get; }
    public bool IsActive { get; private set; }

    protected float duration;
    public GameplayEffectSpec(
            GameplayEffect gameplayEffect,
            AbilitySystemComponent source,
            AbilitySystemComponent owner)
    {
        GameplayEffect = gameplayEffect;
        Source = source;
        Owner = owner;
        duration = gameplayEffect.duration;
        if (DurationPolicy != EffectsDurationPolicy.Instant)
        {
            PeriodTicker = new GameplayEffectPeriodTicker(this);
        }
    }
    public void Tick()
    {
        PeriodTicker?.Tick();
        if (DurationPolicy == EffectsDurationPolicy.Duration && duration.TimerTick())
            RemoveSelf();
    }

    #region TriggerOnSth
    public virtual void TriggerOnTick()
    {

    }
    public virtual void TriggerOnExecute()
    {
        Owner.GameplayEffectContainer.RemoveGameplayEffectWithAnyTags(GameplayEffect.TagContainer
                .RemoveGameplayEffectsWithTags);

        if(!Owner.HasAllTags(GameplayEffect.TagContainer.ApplicationRequiredTags))
            RemoveSelf();
    }
    public virtual void TriggerOnRemove()
    {

    }
    public virtual void TriggerOnAdd()
    {

    }
    public virtual void TriggerOnImmunity()
    {

    }
    
    #endregion

    public void RemoveSelf()
    {
        Owner.GameplayEffectContainer.RemoveGameplayEffectSpec(this);
    }

}
