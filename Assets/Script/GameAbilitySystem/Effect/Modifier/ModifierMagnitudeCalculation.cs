using System.Collections;
using UnityEditor;
using UnityEngine;


public abstract class ModifierMagnitudeCalculation : ScriptableObject
{
    public abstract float CalculateMagnitude(GameplayEffectSpec spec, float modifierMagnitude);
}