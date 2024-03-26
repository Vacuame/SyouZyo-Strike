using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 作为一个角色，有动画和CharacterController来移动
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilitySystemComponent))]
public abstract class Character : Pawn
{
    #region 必要组件
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    protected CharacterController cc;
    //protected HealthComponent healthComp;

    [HideInInspector]public AbilitySystemComponent ABS;
    #endregion

    [Header("绑定"), SerializeField]protected Transform feetTransform;

    protected override void Awake()
    {
        //先找到常用组件
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ABS = GetComponent<AbilitySystemComponent>();

        //设定helthComp
/*        healthComp = GetComponent<HealthComponent>();
        healthComp.OnHealthZero += OnDeadStart;*/

        ABS.Prepare();
    }
    protected virtual void OnHit(HitInfo hitInfo)
    {
        //healthComp.OnHit(ref hitInfo);
    }
    protected abstract void OnDeadStart();
    protected abstract void OnDead();
    protected abstract void OnDeadEnd();
}
