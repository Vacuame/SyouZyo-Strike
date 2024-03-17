using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Character : Pawn
{
    #region 必要组件
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    protected CharacterController cc;
    protected HealthComponent healthComp;

    [HideInInspector]public AbilitySystemComponent ABS;
    #endregion

    #region 生命周期
    protected override void Awake()
    {
        //先找到常用组件
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ABS = GetComponent<AbilitySystemComponent>();

        //设定helthComp
        healthComp = GetComponent<HealthComponent>();
        healthComp.OnHealthZero += OnDeadStart;

        ABS.Prepare();
    }
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {

    }

    #endregion

    protected virtual void OnHit(HitInfo hitInfo)
    {;
        healthComp.OnHit(ref hitInfo);
    }
    protected abstract void OnDeadStart();
    protected abstract void OnDead();
    protected abstract void OnDeadEnd();
}
