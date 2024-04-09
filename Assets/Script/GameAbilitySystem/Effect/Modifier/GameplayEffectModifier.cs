using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public struct GameplayEffectModifier
{
    public string AttributeSetName;

    public string AttributeShortName;

    public float ModiferMagnitude;

    public Tags.Calc Operation;

    public ValueType ValueType;

    public ModifierMagnitudeCalculation MMC;

    public GameplayEffectModifier(
        string attributeSetName,
        string attributeShortName,
        float modiferMagnitude,
        Tags.Calc operation,
        ValueType valueType,
        ModifierMagnitudeCalculation mmc)
    {
        AttributeSetName = attributeSetName;
        AttributeShortName = attributeShortName;
        ModiferMagnitude = modiferMagnitude;
        Operation = operation;
        ValueType = valueType;
        MMC = mmc!=null ? Object.Instantiate(mmc):null;
    }
}