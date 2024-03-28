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
    [HideInInspector] public Animator anim;
    [HideInInspector] public CharacterController cc;
    //protected HealthComponent healthComp;

    [HideInInspector]public AbilitySystemComponent ABS;
    #endregion

    [Header("��"), SerializeField]protected Transform feetTransform;

    protected override void Awake()
    {
        //���ҵ��������
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        ABS = GetComponent<AbilitySystemComponent>();

        //�趨helthComp
/*        healthComp = GetComponent<HealthComponent>();
        healthComp.OnHealthZero += OnDeadStart;*/
    }
    protected abstract void OnHit(HitInfo hitInfo);
    protected abstract void OnDeadStart();
    protected abstract void OnDead();
    protected abstract void OnDeadEnd();
}
