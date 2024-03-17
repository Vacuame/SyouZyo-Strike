using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayTagAggregator : MonoBehaviour
{
    public List<GameplayTag> fixTags;
    public Dictionary<GameplayTag,Object> dynamicTags;

    #region Tag的增删
    public void AddFixTag(GameplayTag tag)
    {

    }

    public void RemoveFixTag(GameplayTag tag) 
    { 

    }

    public void AddDynamicTag(GameplayTag tag,Object source) 
    {

    }

    public void RemoveDynamicTag(GameplayTag tag, Object source) 
    { 
    
    }

    public void ApplyAbilityTags(AbilitySpec source)
    {

    }
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
    public bool HasNoneTags(GameplayTagSet other)
    {
        return other.Empty || !other.Tags.Any(HasTag);
    }

    public bool HasNoneTags(params GameplayTag[] tags)
    {
        return !tags.Any(HasTag);
    }
    #endregion
}
