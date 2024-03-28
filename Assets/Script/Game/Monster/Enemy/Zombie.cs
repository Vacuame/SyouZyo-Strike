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
        CharaAtrr s = new CharaAtrr(Resources.Load<CharaAttr_SO>("ScriptObjectData/Enemy/ZombieAttr"));
        ABS.AttributeSetContainer.AddAttributeSet(s);
    }
}
