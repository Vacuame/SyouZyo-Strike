using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct GameplayTag
{
    [SerializeField] private string _name;
    [SerializeField] private int _hashCode;
    [SerializeField] private string _shortName;
    [SerializeField] private int[] _ancestorHashCodes;
    [SerializeField] private string[] _ancestorNames;//Tag是树状结构，逐层记录上级的名字（a a.b a.b.c）

    public static implicit operator GameplayTag(string name)//适用于没有树形结构的标签，隐式转换比较方便
    {
        return new GameplayTag(TagManager.Instance.GetTagGeneration(name));
    }

    public GameplayTag(string name)
    {
        _name = name;
        _hashCode = name.GetHashCode();

        var tags = name.Split('.');

        _ancestorNames = new string[tags.Length - 1];
        _ancestorHashCodes = new int[tags.Length - 1];
        string ancestorTag = "";

        for (int i = 0; i < tags.Length - 1;i++)
        {
            ancestorTag += tags[i];
            _ancestorHashCodes[i] = ancestorTag.GetHashCode();
            _ancestorNames[i] = ancestorTag;
            ancestorTag += ".";
        }

        _shortName = tags.Last();
    }

    public GameplayTag(List<string>subTags)
    {
        _ancestorNames = new string[subTags.Count - 1];
        _ancestorHashCodes = new int[subTags.Count - 1];
        string ancestorTag = "";

        for (int i = 0; i < subTags.Count - 1; i++)
        {
            ancestorTag += subTags[i];
            _ancestorHashCodes[i] = ancestorTag.GetHashCode();
            _ancestorNames[i] = ancestorTag;
            ancestorTag += ".";
        }

        _shortName = subTags.Last();
        _name = ancestorTag + _shortName;
        _hashCode = _name.GetHashCode();
    }

    #region 读取数据
    public string Name => _name;
    public string ShortName => _shortName;

    public int HashCode => _hashCode;

    public string[] AncestorNames => _ancestorNames;

    public bool Root => _ancestorHashCodes.Length == 0;

    public int[] AncestorHashCodes => _ancestorHashCodes;
    #endregion

    public bool IsAncestorOf(GameplayTag other)
    {
        return other._ancestorHashCodes.Contains(HashCode);
    }

    public override bool Equals(object obj)
    {
        return obj is GameplayTag tag && this == tag;
    }

    public override int GetHashCode()
    {
        return HashCode;
    }

    public static bool operator ==(GameplayTag x, GameplayTag y)
    {
        return x.HashCode == y.HashCode;
    }

    public static bool operator !=(GameplayTag x, GameplayTag y)
    {
        return x.HashCode != y.HashCode;
    }

    /// <summary>
    /// 检查是否属于target类，是的话说明有这种tag
    /// </summary>
    /// <param name="targetTag"></param>
    /// <returns></returns>
    public bool HasTag(GameplayTag targetTag)
    {
        if(targetTag.IsAncestorOf(this)) return true;
/*        foreach (var ancestorHashCode in _ancestorHashCodes)
            if (ancestorHashCode == targetTag.HashCode)
                return true;*/

        return this == targetTag;
    }
}