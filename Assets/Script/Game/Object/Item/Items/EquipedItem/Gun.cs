using Cinemachine;
using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : EquipedItem
{
    //[HideInInspector] public GameObject user;
    [SerializeField] public Transform muzzle, handGuard;
    [SerializeField] private float rateOfGun = 300;
    private bool shooting,aiming;
    private float shootInterval;
    private float timeToNextShoot;
    [SerializeField] LayerMask shootMask;

    [SerializeField] private int _curAmmo,_fullAmmo;
    public int curAmmo { get { return _curAmmo; }set{ _curAmmo = value; HUDManager.GetHUD<AimHUD>()?.SetAmmo(_curAmmo, _fullAmmo); } }
    public int fullAmmo { get { return _fullAmmo; }set{ _fullAmmo = value; HUDManager.GetHUD<AimHUD>()?.SetAmmo(_curAmmo, _fullAmmo); } }

    /// <summary>扩散的屏幕百分比</summary>
    [Header("扩散")]
    private float spread;
    [HideInInspector]public bool moving;
    [SerializeField] private float onAimSpread;
    [SerializeField] private float minSpread,maxSpread;
    private float baseSpread;
    [SerializeField] private float minSpreadShrinkSpeed, maxSpreadShrinkSpeed, spreadShrinkAcc;
    private float spreadShrinkSpeed;
    [SerializeField] private float aimShrinkWait;
    [SerializeField] private float overUsedShrinkWait;
    private float shrinkWaitTimer;

    private CinemachineImpulseSource cameraShakeSource;

    /// <summary>实际的屏幕距离</summary>
    private float sightDistance;
    [SerializeField] private AnimationCurve recoil;

    /// <summary>用作特效的变量</summary>
    Vector3 bulletVector;
    private int gunFireId,bulletTrailId;

    /// <summary>每发子弹对应的y轴位置</summary>
    private List<float> bulletToRecoil=new List<float>();

    private float lastSpreadGrow;
    private void Awake()
    {
        baseSpread = maxSpread - minSpread;
        for (float i= 0; i <= fullAmmo; i++) 
        {
            float curveTime = i / fullAmmo;
            bulletToRecoil.Add(recoil.Evaluate(curveTime));
        }

        cameraShakeSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        HUDManager.GetHUD<AimHUD>()?.SetAmmo(curAmmo, fullAmmo);
    }
    private void OnDisable()
    {
        SetAiming(false);
        //UIManager.Instance.GetHUD<ShootHUD>().SetAmmo(5, 30);
        HUDManager.GetHUD<AimHUD>()?.SetAmmo(-1, -1);
    }

    private void Update()
    {
        if (aiming)
        {
            if(moving&&spread<onAimSpread)
            {
                spread = onAimSpread;
                spreadShrinkSpeed = minSpreadShrinkSpeed;
                shrinkWaitTimer = aimShrinkWait;
            }

            sightDistance = Screen.height * (spread) / 200;

            if (shooting)
            {
                if (timeToNextShoot.TimerTick()) Shoot();
                ParticleManager.Instance.SetKeep(gunFireId, muzzle.position, muzzle.rotation);
                ParticleManager.Instance.SetKeep(bulletTrailId, muzzle.position, Quaternion.LookRotation(bulletVector));
            }
        }

        //不射击缩小准星 或者 准星扩散大于缩小时才可以缩小（否则准星无法增大）
        if(shrinkWaitTimer.TimerTick())
            if (!shooting||lastSpreadGrow > spreadShrinkSpeed * shootInterval)
            {
                float targetSpread = moving ? onAimSpread : minSpread;
                spread = Mathf.MoveTowards(spread, targetSpread, spreadShrinkSpeed * Time.deltaTime);
                spreadShrinkSpeed = Mathf.MoveTowards(spreadShrinkSpeed, maxSpreadShrinkSpeed, spreadShrinkAcc * Time.deltaTime);
            }
    }

    private void LateUpdate()//更新UI
    {
        float sightDis = aiming ? sightDistance : -1;
        HUDManager.GetHUD<AimHUD>()?.SetSightDis(sightDis); 
    }

    public void SetAiming(bool value)
    {
        aiming = value;
        if(aiming)
        {
            if(spread <= onAimSpread)
            {
                spread = onAimSpread;
                shrinkWaitTimer = aimShrinkWait;
            }
            spreadShrinkSpeed = minSpreadShrinkSpeed;
            shootInterval = 60 / rateOfGun;
        }
        else
        {
            TrySetShooting(false);
            HUDManager.GetHUD<AimHUD>()?.SetSightDis(-1);
        }
    }

    public bool TrySetShooting(bool value)
    {
        if (!aiming&&value) return false;

        if(value)
        {
            if (curAmmo <= 0)
            {   
                //TODO 提示无子弹
                return false;
            }
            shooting = value;
            Shoot();
            timeToNextShoot = shootInterval;
            gunFireId = ParticleManager.Instance.GetKeepId();
            bulletTrailId = ParticleManager.Instance.GetKeepId();
            ParticleManager.Instance.PlayEffect("GunFire", muzzle.position,muzzle.rotation,keep:true,keepId:gunFireId);
            ParticleManager.Instance.PlayEffect("BulletTrail", muzzle.position, Quaternion.LookRotation(bulletVector), keep: true, keepId: bulletTrailId);
        }
        else
        {
            shooting = value;
            ParticleManager.Instance.CloseKeep(gunFireId,true);
            ParticleManager.Instance.CloseKeep(bulletTrailId);
        }
        return true;
    }

    private void Shoot()
    {
        if(curAmmo<=0)
        {
            //TODO 提示无子弹
            TrySetShooting(false);
            return;
        }
        --curAmmo;

        timeToNextShoot = shootInterval;
        Vector2 aimPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        aimPoint = Calc.CircleRandomPoint(aimPoint, sightDistance);

        //根据当前spread计算下一发：y轴是spread，将spread转为x轴，需要2分查找。以子弹为单位二分查找哪个子弹对应的y最接近spread
        {
            float recoilValue = (spread - minSpread) / baseSpread;
            int recoilIndex = bulletToRecoil.BinarySearch(recoilValue);//注意List二分查找没找到返回的是补码！
            recoilIndex = recoilIndex >= 0 ? recoilIndex : (~recoilIndex);
            int nextIndex = Mathf.Min(bulletToRecoil.Count - 1, recoilIndex + 1);

            lastSpreadGrow = (bulletToRecoil[nextIndex] - bulletToRecoil[recoilIndex]) * baseSpread;
            float recoilTime = (float)nextIndex / fullAmmo;
            spread = minSpread + recoil.Evaluate(recoilTime) * baseSpread;
        }
        
        if (spread == maxSpread)//惩罚机制
            shrinkWaitTimer = overUsedShrinkWait;

        spreadShrinkSpeed = minSpreadShrinkSpeed;

        Ray shootRay = Camera.main.ScreenPointToRay(aimPoint);
        bulletVector = (Camera.main.transform.position + shootRay.direction * 200 - muzzle.position).normalized;
        if (Physics.Raycast(shootRay, out RaycastHit hit, 200f, shootMask))
        {
            bulletVector = (hit.point - muzzle.position).normalized;
            Vector3 randDir = new Vector3(Random.Range(-1, 1),
                Random.Range(-1, 1),Random.Range(-1, 1));
            Vector3 dir = bulletVector*-1 + randDir * 0.3f;

            GameObject hitObj = hit.collider.gameObject;

            //如果是这些layer就播放默认特效
            if (hitObj.layer == 0 || hitObj.layer == LayerMask.NameToLayer("Climbable"))
                ParticleManager.Instance.PlayEffect("BulletImpact" ,hit.point, Quaternion.LookRotation(dir));
            
            if(hitObj.layer == LayerMask.NameToLayer("EnemyPart"))
            {
                EventManager.Instance.TriggerEvent("Hit" + hitObj.GetInstanceID(),
                    new HitInfo(HitType.SMG , 10, user.gameObject, hitObj, hit.point, dir));
            }
        }

        Vector3 shakeVector = cameraShakeSource.m_DefaultVelocity;
        shakeVector.x = Random.Range(-0.1f, 0.1f);
        cameraShakeSource.GenerateImpulse(shakeVector*0.1f);
    }
}
