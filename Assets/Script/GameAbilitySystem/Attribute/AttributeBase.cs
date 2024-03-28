using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttributeBase
{
    public readonly string FullName;
    public readonly string SetName;
    public readonly string ShortName;
    public event Action<AttributeBase, float, float> onPostCurrentValueChange;
    public event Action<AttributeBase, float, float> onPostBaseValueChange;
    public event Func<AttributeBase, float, float> onPreCurrentValueChange;
    public event Func<AttributeBase, float, float> onPreBaseValueChange;
    private AttributeValue attrValue;
    public AttributeValue Value => attrValue;
    public float BaseValue => attrValue.baseValue;
    public float CurrentValue => attrValue.currentValue;

    public static implicit operator float(AttributeBase b) => b.CurrentValue;

    public AttributeBase(string attrSetName, string attrName, float baseValue = 0)
    {
        SetName = attrSetName;
        FullName = $"{attrSetName}.{attrName}";
        ShortName = attrName;
        attrValue = new AttributeValue(baseValue);
    }

    public void SetCurValueRelative(float input,Tags.Calc calcType)
    {
        float value = CurrentValue;
        switch (calcType) 
        {
            case Tags.Calc.Add:
                value = CurrentValue + input;
                break;
            case Tags.Calc.Sub:
                value = CurrentValue - input;
                break;
            case Tags.Calc.Mul:
                value = CurrentValue * input;
                break;
            case Tags.Calc.Div:
                value = CurrentValue / input;
                break;
        }
        SetCurValue(value);
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
    public void RefreshCurValue() => SetCurValue(BaseValue);

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
