using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获得数据集->初始化->绑定事件
/// </summary>
public class AttributeSetContainer
{
    private readonly AbilitySystemComponent _owner;
    private readonly Dictionary<string, AttributeSet> _attributeSets = new Dictionary<string, AttributeSet>();
    public Dictionary<string, AttributeSet> Sets => _attributeSets;

    public AttributeSetContainer(AbilitySystemComponent owner)
    {
        _owner = owner;
        
    }

    #region AttributeSet的增删查

    #region 查
    public bool TryGetAttributeSet<T>(out T attributeSet) where T : AttributeSet
    {
        if (_attributeSets.TryGetValue(typeof(T).Name, out var set))
        {
            attributeSet = (T)set;
            return true;
        }
        attributeSet = null;
        return false;
    }
    bool TryGetAttributeSet(string attrSetName, out AttributeSet attributeSet)
    {
        if (_attributeSets.TryGetValue(attrSetName, out var set))
        {
            attributeSet = set;
            return true;
        }

        attributeSet = null;
        return false;
    }
    #endregion

    #region 增
    public void AddAttributeSet<T>() where T : AttributeSet
    {
        if (TryGetAttributeSet<T>(out _)) return;

        _attributeSets.Add(nameof(T), Activator.CreateInstance<T>());
    }
    public void AddAttributeSet(AttributeSet attributeSet)
    {
        if (TryGetAttributeSet(attributeSet.SetName, out _)) return;

        _attributeSets.Add(attributeSet.SetName, attributeSet);
    }
    #endregion

    #region 删
    public void RemoveAttributeSet<T>() where T : AttributeSet
    {
        _attributeSets.Remove(nameof(T));
    }
    #endregion

    #endregion

    #region 直接通过名字拿到属性
    public AttributeBase GetAttributeBase(string attrSetName, string attrShortName)
    {
        return _attributeSets.TryGetValue(attrSetName, out var set) ? set[attrShortName] : null;
    }
    public float? GetAttributeBaseValue(string attrSetName, string attrShortName)
    {
        return _attributeSets.TryGetValue(attrSetName, out var set) ? set[attrShortName].BaseValue : (float?)null;
    }

    public float? GetAttributeCurrentValue(string attrSetName, string attrShortName)
    {
        return _attributeSets.TryGetValue(attrSetName, out var set) ? set[attrShortName].CurrentValue : (float?)null;
    }
    #endregion
}
