using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


[Serializable]
public struct GameplayEffectModifier
{
    public string AttributeName;

    [HideInInspector]
    public string AttributeSetName;

    [HideInInspector]
    public string AttributeShortName;

    public float ModiferMagnitude;

    public Tags.Calc Operation;

    public ValueType ValueType;

    public ModifierMagnitudeCalculation MMC;

    public GameplayEffectModifier(
        string attributeName,
        float modiferMagnitude,
        Tags.Calc operation,
        ValueType valueType,
        ModifierMagnitudeCalculation mmc)
    {
        AttributeName = attributeName;
        var splits = attributeName.Split('.');
        AttributeSetName = splits[0];
        AttributeShortName = splits[1];
        ModiferMagnitude = modiferMagnitude;
        Operation = operation;
        ValueType = valueType;
        MMC = mmc;
    }
}