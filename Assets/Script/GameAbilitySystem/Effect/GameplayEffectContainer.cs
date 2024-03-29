using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayEffectContainer
{
    private readonly AbilitySystemComponent _owner;

    private readonly List<GameplayEffectSpec> _gameplayEffectSpecs = new List<GameplayEffectSpec>();
    public GameplayEffectContainer(AbilitySystemComponent owner)
    {
        _owner = owner;
    }

    public void Tick()
    {
        var enumerable = _gameplayEffectSpecs.ToArray();
        foreach (var gameplayEffectSpec in enumerable)
        {
            gameplayEffectSpec.Tick();
        }
    }
    public bool AddGameplayEffectSpec(GameplayEffectSpec spec)
    {
        if (_owner.HasAnyTags(spec.GameplayEffect.TagContainer.ApplicationImmunityTags))
        {
            spec.TriggerOnImmunity();
            return false;
        }

        if (spec.GameplayEffect.durationPolicy == EffectsDurationPolicy.Instant)
        {
            spec.TriggerOnExecute();
            return false;
        }

        _gameplayEffectSpecs.Add(spec);
        spec.TriggerOnAdd();

        return true;
    }
    public void RemoveGameplayEffectSpec(GameplayEffectSpec spec)
    {
        spec.TriggerOnRemove();
        _gameplayEffectSpecs.Remove(spec);
    }
    public void ClearGameplayEffect()
    {
        foreach (var gameplayEffectSpec in _gameplayEffectSpecs)
        {
            gameplayEffectSpec.TriggerOnRemove();
        }

        _gameplayEffectSpecs.Clear();
    }

    internal void RemoveGameplayEffectWithAnyTags(GameplayTagSet tags)
    {
        if (tags.Empty) return;

        var removeList = new List<GameplayEffectSpec>();
        foreach (var gameplayEffectSpec in _gameplayEffectSpecs)
        {
            var assetTags = gameplayEffectSpec.GameplayEffect.TagContainer.AssetTags;
            if (!assetTags.Empty && assetTags.HasAnyTags(tags))
            {
                removeList.Add(gameplayEffectSpec);
                continue;
            }
        }

        foreach (var gameplayEffectSpec in removeList)
            RemoveGameplayEffectSpec(gameplayEffectSpec);
    }
}
