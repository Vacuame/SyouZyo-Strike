using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeSetContainer
{
    private readonly AbilitySystemComponent _owner;
    private readonly Dictionary<string, AttributeSet> _attributeSets = new Dictionary<string, AttributeSet>();
    public Dictionary<string, AttributeSet> Sets => _attributeSets;

    public AttributeSetContainer(AbilitySystemComponent owner)
    {
        _owner = owner;
        
    }

    #region AttributeSet����ɾ��
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
    public void AddAttributeSet<T>() where T : AttributeSet
    {
        if (TryGetAttributeSet<T>(out _)) return;

        _attributeSets.Add(nameof(T), Activator.CreateInstance<T>());
    }
    public void RemoveAttributeSet<T>() where T : AttributeSet
    {
        _attributeSets.Remove(nameof(T));
    }
    #endregion

    #region ֱ��ͨ�������õ�����
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
