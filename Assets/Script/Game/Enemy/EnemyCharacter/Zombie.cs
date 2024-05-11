using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    [Header("π•ª˜…Ë÷√")]
    [SerializeField]private BoxCollider atkRange_TwoHand;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player fb_hitOnCut;
    [SerializeField] private MMF_Player fb_hitOnImpulse;
    [SerializeField] private MMF_Player fb_parried;

    [Header("Sound")]
    [SerializeField] private AudioClip soundOnBullet;
    [SerializeField] private AudioClip soundOnCut, soundOnImpulse,soundOnParried;
    [SerializeField] private AudioClip deadSound;
    protected override void Awake()
    {
        base.Awake();
        EnemyNormalAttackAsset atkData_TwoHand = Resources.Load<EnemyNormalAttackAsset>("ScriptObjectData/Enemy/Zombie/Zombie_Attack_TwoHand");
        ABS.GrandAbility(new EnemyNormalAttack(atkData_TwoHand,this,atkRange_TwoHand));
        EnemyNormalAttackAsset atkData_Light = Resources.Load<EnemyNormalAttackAsset>("ScriptObjectData/Enemy/Zombie/Zombie_Attack_Light");
        ABS.GrandAbility(new EnemyNormalAttack(atkData_Light, this, atkRange_TwoHand));
        EnemyNormalAttackAsset atkData_Heavy = Resources.Load<EnemyNormalAttackAsset>("ScriptObjectData/Enemy/Zombie/Zombie_Attack_Heavy");
        ABS.GrandAbility(new EnemyNormalAttack(atkData_Heavy, this, atkRange_TwoHand));
    }

    protected override void PlayHitEffect(HitInfo hitInfo)
    {
        HitType type = hitInfo.type;
        AudioClip audioClip;

        if(type == HitType.Parry)
        {
            fb_parried.PlayFeedbacks();
            audioClip = soundOnParried;
        }
        else if(type == HitType.Impulse || type == HitType.Explode)
        {
            fb_hitOnImpulse.PlayFeedbacks();
            audioClip = soundOnImpulse;
        }
        else
        {
            ParticleManager.GetOrCreateInstance()?.PlayEffect("BulletImpact_Blood", hitInfo.pos, Quaternion.LookRotation(hitInfo.dire * -1));

            if(type == HitType.Cut)
            {
                fb_hitOnCut.PlayFeedbacks();
                audioClip = soundOnCut;
            }
            else
            {
                audioClip = soundOnBullet;
            }
        }

        SoundManager.GetOrCreateInstance()?.PlaySound(SoundPoolType.SFX, audioClip, hitInfo.pos);
    }

    public override void Dead()
    {
        base.Dead();

        SoundManager.Instance.PlaySound(SoundPoolType.SFX, deadSound,transform.position);
    }

}
