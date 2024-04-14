using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum ValueType
{
    Base,Cur
}

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

    public AttributeBase(string attrSetName, string attrName, float baseValue)
    {
        SetName = attrSetName;
        FullName = $"{attrSetName}.{attrName}";
        ShortName = attrName;
        attrValue = new AttributeValue(baseValue);
    }

    /// <summary>
    /// 相对计算数值，默认计算CurValue
    /// </summary>
    public void SetValueRelative(float input,Tags.Calc calcType,ValueType valueType = ValueType.Cur)
    {
        float value;
        if (valueType == ValueType.Cur) 
            value = CurrentValue;
        else 
            value = BaseValue;

        switch (calcType) 
        {
            case Tags.Calc.Add:
                value = value + input;
                break;
            case Tags.Calc.Sub:
                value = value - input;
                break;
            case Tags.Calc.Mul:
                value = value * input;
                break;
            case Tags.Calc.Div:
                value = value / input;
                break;
            case Tags.Calc.Override: 
                value = input;
                break;
        }
        if (valueType == ValueType.Cur)
            SetCurValue(value);
        else
            SetBaseValue(value);
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
    public float GetProportion()
    {
        return CurrentValue / BaseValue;
    }
    public virtual void Dispose()
    {
        onPreBaseValueChange = null;
        onPostBaseValueChange = null;
        onPreCurrentValueChange = null;
        onPostCurrentValueChange = null;
    }

}
