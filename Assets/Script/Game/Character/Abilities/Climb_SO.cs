using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClimbData", menuName = "Ability/Climb")]
public class Climb_SO : AbilityAsset
{
    [Header(" ˝æ›…Ë÷√")]
    public float minClimbHeight;
    public float
        midClimbHeight, 
        maxClimbHeight, 
        climbStep,
        climbCheckDistance, 
        climbOverDistance,
        climbTowardAngle;

    public LayerMask climbLayer;

}
