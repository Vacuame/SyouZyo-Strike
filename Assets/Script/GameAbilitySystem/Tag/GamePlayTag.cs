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
    [SerializeField] private string[] _ancestorNames;//Tag����״�ṹ������¼�ϼ������֣�a a.b a.b.c��

    public GameplayTag(string name)
    {
        _name = name;
        _hashCode = name.GetHashCode();

        var tags = name.Split('.');

        _ancestorNames = new string[tags.Length - 1];
        _ancestorHashCodes = new int[tags.Length - 1];
        int i = 0;
        string ancestorTag = "";
        while (i < tags.Length - 1)
        {
            ancestorTag += tags[i];
            _ancestorHashCodes[i] = ancestorTag.GetHashCode();
            _ancestorNames[i] = ancestorTag;
            ancestorTag += ".";
            i++;
        }

        _shortName = tags.Last();
    }

    public string Name => _name;
    public string ShortName => _shortName;

    public int HashCode => _hashCode;

    public string[] AncestorNames => _ancestorNames;

    public bool Root => _ancestorHashCodes.Length == 0;

    public int[] AncestorHashCodes => _ancestorHashCodes;

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

    public bool HasTag(GameplayTag tag)
    {
        foreach (var ancestorHashCode in _ancestorHashCodes)
            if (ancestorHashCode == tag.HashCode)
                return true;

        return this == tag;
    }
}