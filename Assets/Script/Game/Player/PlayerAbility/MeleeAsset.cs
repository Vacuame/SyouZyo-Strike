using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 每个招式的时间设置
/// </summary>
[CreateAssetMenu(fileName = "NewMeleeData", menuName = "ABS/Ability/Melee")]
public class MeleeAsset : AbilityAsset
{
    [Header("连击设置")]
    public List<MeleeConfig> meleeConfigs = new List<MeleeConfig>();

    public string animParamName;
    public float nextMeleeEarlierClickTime;//在不能连招的时候按了也能执行

    [System.Serializable]
    public struct MeleeConfig
    {
        public float animSpeed;
        public float damageMultiplier;

        public float atkStartTime;
        public float atkEndTime;
        public float canNextTime;
        public float meleeEndTime;
    }

}
