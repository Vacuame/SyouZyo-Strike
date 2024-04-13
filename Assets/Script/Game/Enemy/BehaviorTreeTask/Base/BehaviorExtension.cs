using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class BehaviorExtension
{
    public static void Restart(this BehaviorTree bt)
    {
        bt.DisableBehavior();
        bt.EnableBehavior();
    }
}
