using System;
using System.Collections;
using System.Collections.Generic;
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

    private List<GameplayCueDurationalSpec> _cueDurationalSpecs = new List<GameplayCueDurationalSpec>();

    public event Action<AbilitySystemComponent, GameplayEffectSpec> onImmunity;

    public float duration;
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
        TriggerOnTick();

        PeriodTicker?.Tick();

        if (DurationPolicy == EffectsDurationPolicy.Duration && duration.TimerTick())
            RemoveSelf();
    }

    #region TriggerOnSth

    /// <summary>
    /// 每帧执行
    /// </summary>
    public virtual void TriggerOnTick()
    {
        if (DurationPolicy == EffectsDurationPolicy.Duration ||
            DurationPolicy == EffectsDurationPolicy.Infinite)
        {
            if (GameplayEffect.CueDurational != null && GameplayEffect.CueDurational.Length > 0)
            {
                foreach (var cue in _cueDurationalSpecs)
                    cue.OnTick();
            }
        }
    }

    /// <summary>
    /// 当buff间隔到了执行一次
    /// </summary>
    public virtual void TriggerOnExecute()
    {
        TriggerInstantCues(GameplayEffect.CueOnExecute);

        Owner.GameplayEffectContainer.RemoveGameplayEffectWithAnyTags(GameplayEffect.TagContainer
                .RemoveGameplayEffectsWithTags);

        Owner.ApplyModFromInstantGameplayEffect(this);
    }
    public virtual void TriggerOnRemove()
    {
        //执行CueOnRemove
        if (GameplayEffect.CueOnRemove != null && GameplayEffect.CueOnRemove.Length > 0)
            TriggerInstantCues(GameplayEffect.CueOnRemove);

        if (GameplayEffect.CueDurational != null && GameplayEffect.CueDurational.Length > 0)
        {
            foreach (var cue in _cueDurationalSpecs) cue.OnRemove();

            _cueDurationalSpecs = null;
        }
    }
    public virtual void TriggerOnAdd()
    {
        //执行CueOnAdd
        if (GameplayEffect.CueOnAdd != null && GameplayEffect.CueOnAdd.Length > 0)
            TriggerInstantCues(GameplayEffect.CueOnAdd);

        //重新加载CueDurational，并且OnAdd
        if (GameplayEffect.CueDurational != null && GameplayEffect.CueDurational.Length > 0)
        {
            _cueDurationalSpecs.Clear();
            foreach (var cueDurational in GameplayEffect.CueDurational)
            {
                var cueSpec = cueDurational.ApplyFrom(this);
                if (cueSpec != null) _cueDurationalSpecs.Add(cueSpec);
            }

            foreach (var cue in _cueDurationalSpecs) cue.OnAdd();
        }
    }
    public virtual void TriggerOnImmunity()
    {
        onImmunity?.Invoke(Owner, this);
        onImmunity = null;
    }

    #endregion

    #region Cue
    void TriggerInstantCues(GameplayCueInstant[] cues)
    {
        if (cues == null) return;
        foreach (var cue in cues) cue.ApplyFrom(this);
    }
    #endregion

    public void RemoveSelf()
    {
        Owner.GameplayEffectContainer.RemoveGameplayEffectSpec(this);
    }

}
