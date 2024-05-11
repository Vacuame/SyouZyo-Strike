using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParryAsset", menuName = "ABS/Ability/Parry")]
public class ParryAsset : AbilityAsset
{
    public Collider parryColPrefab;

    public float perfectParryDmg;

    public float perfectParryStTime;
    public float perfectParryEdTime;

    public GameplayEffectAsset perfectParryEffect;
}
