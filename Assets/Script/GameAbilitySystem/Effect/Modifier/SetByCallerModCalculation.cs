using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SetByCallerModCalculation", menuName = "ABS/MMC/SetByCallerModCalculation")]
public class SetByCallerModCalculation : ModifierMagnitudeCalculation
{
    public override float CalculateMagnitude(GameplayEffectSpec spec, float input)
    {
        return input;
    }
}