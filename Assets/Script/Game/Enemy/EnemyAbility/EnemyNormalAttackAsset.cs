using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "ABS/Ability/Attack")]
public class EnemyNormalAttackAsset : AbilityAsset
{
    [Header("��������")]
    public AnimPlayConfig animPara;

    public LayerMask targetMask;

    public List<AttackConfig>atkConfigs = new List<AttackConfig>();

    [System.Serializable]
    public struct AttackConfig
    {
        public float dmg;
        public int makeDmgFrame;
    }
    [Header("������Ϣ")]
    public int animStartFrame;
    public int animEndFrame;
    public float animLenth => (float)(animEndFrame - animStartFrame) / 30;
}
