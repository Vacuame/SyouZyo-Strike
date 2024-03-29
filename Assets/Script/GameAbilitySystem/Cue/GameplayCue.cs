using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 和Ability原理差不多的东西
/// </summary>
public abstract class GameplayCue : ScriptableObject
{
    public string[] RequiredTags;
    public string[] ImmunityTags;

    public virtual bool Triggerable(AbilitySystemComponent owner)
    {
        if (owner == null) return false;

        if (!owner.HasAllTags(new GameplayTagSet(RequiredTags)))//有所需要的
            return false;

        if (owner.HasAnyTags(new GameplayTagSet(ImmunityTags)))//无被免疫的
            return false;

        return true;
    }
    
}

public abstract class GameplayCue<T> : GameplayCue where T : GameplayCueSpec
{
    public abstract T CreateSpec(GameplayCueParameters parameters);
}