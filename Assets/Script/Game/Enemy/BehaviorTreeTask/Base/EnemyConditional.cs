using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;



public class EnemyConditional : Conditional
{
    protected CharacterController cc;
    protected Enemy me;

    public override void OnAwake()
    {
        me = GetComponent<Enemy>();
        cc = me.cc;
    }
}
