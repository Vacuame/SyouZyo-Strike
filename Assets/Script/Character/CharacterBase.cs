using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HealthComponent))]
public abstract class CharacterBase : MonoBehaviour
{
    //需要设置的
    [SerializeField,Header("资源设置")]
    public Animator anim;

    //必要组件
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
