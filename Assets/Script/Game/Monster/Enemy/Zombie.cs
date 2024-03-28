using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        Attack_SO atkData = Resources.Load<Attack_SO>("ScriptObjectData/Enemy/ZombieAttack");
        ABS.GrandAbility(new Attack(atkData));
    }
}
