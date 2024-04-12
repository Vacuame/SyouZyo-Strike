using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "ABS/Ability/Attack")]
public class Attack_SO : AbilityAsset
{
    public AnimPlay animPara;

    public LayerMask targetMask;

    public float dmg;

    public float makeDmgTime, endTime;
}
