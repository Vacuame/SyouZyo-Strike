using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AssassinationAsset", menuName = "ABS/Ability/Assassination")]
public class AssassinationAsset : AbilityAsset
{
    public LayerMask assassinLayer;

    public float doAtkTime;
    public float endTime;
    public float dmg;

    public float canAssassinateAngle;
}
