using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttributeBase
{
    public readonly string Name;
    public readonly string SetName;
    public readonly string ShortName;
    protected event Action<AttributeBase, float, float> onPostCurrentValueChange;
    protected event Action<AttributeBase, float, float> onPostBaseValueChange;
    protected event Func<AttributeBase, float, float> onPreCurrentValueChange;
    protected event Func<AttributeBase, float, float> onPreBaseValueChange;
    private AttributeValue attrValue;
    public AttributeValue Value => attrValue;
    public float BaseValue => attrValue.baseValue;
    public float CurrentValue => attrValue.currentValue;

    public AttributeBase(string attrSetName, string attrName, float baseValue)
    {
        SetName = attrSetName;
        Name = $"{attrSetName}.{attrName}";
        ShortName = attrName;
        attrValue = new AttributeValue(baseValue);
    }

    public AttributeBase(string attrSetName, string attrName)
    {
        SetName = attrSetName;
        Name = $"{attrSetName}.{attrName}";
        ShortName = attrName;
        attrValue = new AttributeValue(0);
    }

    public void SetCurValue(float value ,bool excutePreEvent = true,bool excutePostAction = true)
    {
        if(excutePreEvent && onPreCurrentValueChange != null)
            value = onPreCurrentValueChange.Invoke(this, value);

        float oldValue = attrValue.currentValue;
        attrValue.currentValue = value;

        if(excutePostAction)
            onPostCurrentValueChange?.Invoke(this,oldValue, value);
    }

    public void SetBaseValue(float value, bool excutePreEvent = true, bool excutePostAction = true)
    {
        if (excutePreEvent && onPreBaseValueChange != null)
            value = onPreBaseValueChange.Invoke(this, value);

        float oldValue = attrValue.baseValue;
        attrValue.baseValue = value;

        if (excutePostAction)
            onPostBaseValueChange?.Invoke(this, oldValue, value);
    }
    public virtual void Dispose()
    {
        onPreBaseValueChange = null;
        onPostBaseValueChange = null;
        onPreCurrentValueChange = null;
        onPostCurrentValueChange = null;
    }

}
