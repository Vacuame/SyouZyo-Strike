using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    [Header("π•ª˜…Ë÷√")]
    [SerializeField]private BoxCollider atkRange_TwoHand;
    protected override void Awake()
    {
        base.Awake();
        EnemyNormalAttackAsset atkData1 = Resources.Load<EnemyNormalAttackAsset>("ScriptObjectData/Enemy/Zombie_Attack_TwoHand");
        ABS.GrandAbility(new EnemyNormalAttack(atkData1,this,atkRange_TwoHand));
    }
}
