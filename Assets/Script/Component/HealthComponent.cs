using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private SO_Health healthSetting;
    private SO_Health health;
    public UnityAction OnHealthZero;
    private void Awake()
    {
        health = healthSetting?new SO_Health(ref healthSetting):new SO_Health();
    }
    public void OnHit(ref HitInfo info)
    {
        if (health.curHealth < 0) return;//ÎÞµÐÔòÍË³ö

        health.curHealth -= info.damage;
        if (health.curHealth < 0)
            OnHealthZero();
    }

}
