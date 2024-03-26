using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��Ϊһ����ɫ���ж�����CharacterController���ƶ�
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilitySystemComponent))]
public abstract class Character : Pawn
{
    #region ��Ҫ���
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    protected CharacterController cc;
    //protected HealthComponent healthComp;

    [HideInInspector]public AbilitySystemComponent ABS;
    #endregion

    [Header("��"), SerializeField]protected Transform feetTransform;

    protected override void Awake()
    {
        //���ҵ��������
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ABS = GetComponent<AbilitySystemComponent>();

        //�趨helthComp
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
