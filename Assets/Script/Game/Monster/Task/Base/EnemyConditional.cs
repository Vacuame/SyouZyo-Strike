using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;



public class EnemyConditional : Conditional
{
    protected Rigidbody rb;
    protected Enemy me;

    public override void OnAwake()
    {
        me = GetComponent<Enemy>();
        rb = me.rb;

    }
}
