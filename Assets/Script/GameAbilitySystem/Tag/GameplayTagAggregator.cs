using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.UI.GridLayoutGroup;

public class GameplayTagAggregator
{
    public List<GameplayTag> fixTags = new List<GameplayTag>();//角色固有Tag
    public Dictionary<GameplayTag, List<object>> dynamicTags = new Dictionary<GameplayTag, List<object>>();//动作附加Tag
    private AbilitySystemComponent _owner;

    public GameplayTagAggregator(AbilitySystemComponent owner)
    {
        _owner = owner;
    }

    #region Tag的增删

    #region Fix
    public void AddFixTag(GameplayTag tag)
    {
        if (fixTags.Contains(tag)) return;
        fixTags.Add(tag);
    }

    public void AddFixTag(GameplayTagSet tagSet)
    {
        foreach(var a in tagSet.Tags)
            AddFixTag(a);
    }

    public void RemoveFixTag(GameplayTag tag) 
    {
        if (fixTags.Contains(tag))
            fixTags.Remove(tag);
    }
    #endregion

    #region Dynamic
    public void AddDynamicTag(GameplayTag tag,object source) 
    {
        if(dynamicTags.ContainsKey(tag))
        {
            if (!dynamicTags[tag].Contains(source))
                dynamicTags[tag].Add(source);
        }
        else
        {
            dynamicTags.Add(tag, new List<object> { source });
        } 
    }

    public void RemoveDynamicTag(GameplayTag tag, object source) 
    {
        if (dynamicTags.ContainsKey(tag))
        {
            if (dynamicTags[tag].Contains(source))
            {
                dynamicTags[tag].Remove(source);
                if (dynamicTags[tag].Count == 0)
                    dynamicTags.Remove(tag);
            } 
        }
    }

    public void RestoreDynamicTags(GameplayTagSet set,object source)
    {
        foreach(var a in set.Tags)
            RemoveDynamicTag(a, source);
    }
    #endregion

    #region Ability
    public void ApplyAbilityTags(AbilitySpec source)
    {
        foreach(var a in source.ability.Tag.ActivationOwnedTag.Tags)
        {
            AddDynamicTag(a, source);
        }
    }
    public void RestoreAbilityTags(AbilitySpec source)=>
        RestoreDynamicTags(source.ability.Tag.ActivationOwnedTag, source);

    #endregion

    #region Effect
    public void ApplyGameplayEffectTags(GameplayEffectSpec source)
    {
        var grantedTagSet = source.GameplayEffect.TagContainer.GrantedTags;
        foreach (var tag in grantedTagSet.Tags)
        {
            AddDynamicTag(tag,source);
        }
    }

    public void RestoreGameplayEffectTags(GameplayEffectSpec source)=>
        RestoreDynamicTags(source.GameplayEffect.TagContainer.GrantedTags,source);


    #endregion

    #endregion

    #region Tag的查
    public bool HasTag(GameplayTag tag)
    {
        return fixTags.Contains(tag)||dynamicTags.ContainsKey(tag);
    }

    public bool HasAllTags(GameplayTagSet tags)
    {
        return tags.Empty || tags.Tags.All(HasTag);

    }

    public bool HasAnyTags(GameplayTagSet tags)
    {
        return !tags.Empty && tags.Tags.Any(HasTag);
    }
    public bool HasNoneTags(GameplayTagSet tags)
    {
        return tags.Empty || !tags.Tags.Any(HasTag);
    }

    public bool HasNoneTags(params GameplayTag[] tags)
    {
        return !tags.Any(HasTag);
    }
    #endregion
}
