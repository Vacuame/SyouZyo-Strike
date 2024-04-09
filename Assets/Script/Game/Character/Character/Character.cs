using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using GameBasic;
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
    public bool bDead { get; protected set; }

    [HideInInspector]public AbilitySystemComponent ABS;
    #endregion

    [Header("��"), SerializeField]protected Transform feetTransform;

    [SerializeField] private CharaAttr_SO characterAttribute;
    protected override void Awake()
    {
        //���ҵ��������
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        ABS = GetComponent<AbilitySystemComponent>();
        ABS.Prepare();

        ABS.AttributeSetContainer.AddAttributeSet(new CharaAttr(characterAttribute));
        ABS.AttrSet<CharaAttr>().health.onPostCurrentValueChange += OnHealthPost;

    }
    protected virtual void OnHealthPost(AttributeBase health, float old, float now)
    {
        if (old > 0 && now <= 0)
        {
            Dead();
        }
    }
    protected abstract void OnHit(HitInfo hitInfo);
    protected abstract void Dead();
    protected abstract void OnDeadEnd();
}
