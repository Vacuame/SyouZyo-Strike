using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthComponent))]
public abstract class CharacterBase : MonoBehaviour
{
    //��Ҫ���õ�
    [SerializeField,Header("��Դ����")]
    public Animator anim;

    //��Ҫ���
    protected CharacterController cc;
    public Rigidbody rb;
    protected HealthComponent healthComp;
    
    protected virtual void Awake()
    {
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        healthComp.OnHealthZero += OnDeadStart;
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }
    protected virtual void OnHit(HitInfo hitInfo)
    {;
        healthComp.OnHit(ref hitInfo);
    }
    protected abstract void OnDeadStart();
    protected abstract void OnDead();
    protected abstract void OnDeadEnd();
}
