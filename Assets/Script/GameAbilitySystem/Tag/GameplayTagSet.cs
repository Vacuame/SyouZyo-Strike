using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct GameplayTagSet
{
    public readonly GameplayTag[] Tags;

    public bool Empty => Tags.Length == 0;

    public GameplayTagSet(string[] shortNames)
    {
        Tags = new GameplayTag[shortNames.Length];
        for (var i = 0; i < shortNames.Length; i++)
        {
            Tags[i] = new GameplayTag(TagManager.Instance.GetTagGeneration(shortNames[i]));
        }
    }

    public GameplayTagSet(params GameplayTag[] tags)
    {
        Tags = tags ?? Array.Empty<GameplayTag>();
    }

    #region Tag¼ì²é
    public bool HasTag(GameplayTag tag)
    {
        return Tags.Any(t => t.HasTag(tag));
    }

    public bool HasAllTags(GameplayTagSet other)
    {
        return other.Tags.All(HasTag);
    }

    public bool HasAllTags(params GameplayTag[] tags)
    {
        return tags.All(HasTag);
    }

    public bool HasAnyTags(GameplayTagSet other)
    {
        return other.Tags.Any(HasTag);
    }

    public bool HasAnyTags(params GameplayTag[] tags)
    {
        return tags.Any(HasTag);
    }

    public bool HasNoneTags(GameplayTagSet other)
    {
        return !other.Tags.Any(HasTag);
    }

    public bool HasNoneTags(params GameplayTag[] tags)
    {
        return !tags.Any(HasTag);
    }
    #endregion

}
