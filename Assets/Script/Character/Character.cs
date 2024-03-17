using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Character : Pawn
{
    #region ��Ҫ���
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    protected CharacterController cc;
    protected HealthComponent healthComp;

    public AbilitySystemComponent ABS;
    #endregion

    #region ��������
    protected override void Awake()
    {
        //���ҵ��������
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //�趨helthComp
        healthComp = GetComponent<HealthComponent>();
        healthComp.OnHealthZero += OnDeadStart;
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
