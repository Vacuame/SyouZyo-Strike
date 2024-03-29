using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Abilityԭ����Ķ���
/// </summary>
public abstract class GameplayCue : ScriptableObject
{
    public string[] RequiredTags;
    public string[] ImmunityTags;

    public virtual bool Triggerable(AbilitySystemComponent owner)
    {
        if (owner == null) return false;

        if (!owner.HasAllTags(new GameplayTagSet(RequiredTags)))//������Ҫ��
            return false;

        if (owner.HasAnyTags(new GameplayTagSet(ImmunityTags)))//�ޱ����ߵ�
            return false;

        return true;
    }
    
}

public abstract class GameplayCue<T> : GameplayCue where T : GameplayCueSpec
{
    public abstract T CreateSpec(GameplayCueParameters parameters);
}