using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlertData", menuName = "ABS/Attribute/Alert")]
public class AlertAttrAsset : AttributeAsset
{
    public float alertToCheck;
    public float alertToFind;
    public AnimationCurve alertGrowthSpeedCurve;
    public GameplayEffectAsset reduceAlertEffect;

    public float searchTime;
}
