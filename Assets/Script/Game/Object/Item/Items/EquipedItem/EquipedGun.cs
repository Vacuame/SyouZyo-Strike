using Cinemachine;
using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static InventoryStatic;

public class EquipedGun : EquipedItem
{
    #region 变量
    //基本变量
    [Header("位置")]
    public Transform muzzle;
    public Transform handGuard;

    private Camera playerCamera;

    private bool shooting, aiming;

    [Header("枪械设置")]
    //伤害
    [SerializeField] private LayerMask shootMask;
    [SerializeField] private HitType hitType;
    [SerializeField] private float damage;

    //自动
    [SerializeField] private bool automatic;

    //射击时间
    [SerializeField] private float ratePerMinute = 300;
    private float shootInterval;
    private float shootTimer;
    //扩散冷却时间
    private float coolDownInterval;
    private float coolDownTimer;
    [SerializeField] private float coolDownTimeOnAim;

    //子弹
    private int _curAmmo;
    [SerializeField] private int _fullAmmo;
    public int curAmmo { get { return _curAmmo; } set { _curAmmo = value; HUDManager.GetHUD<AimHUD>()?.SetAmmo(_curAmmo, _fullAmmo); } }
    public int fullAmmo { get { return _fullAmmo; } set { _fullAmmo = value; HUDManager.GetHUD<AimHUD>()?.SetAmmo(_curAmmo, _fullAmmo); } }

    // 扩散
    private float spread;
    private float heat;
    private float minHeat, maxHeat;
    public float sightDistance => Screen.height * spread * spreadMultiplier / 200f;

    [SerializeField] float onAimHeat;

    [SerializeField] private AnimationCurve heatToSpreadCurve;
    [SerializeField] private AnimationCurve heatToHeatPerShotCurve;
    [SerializeField] private AnimationCurve heatToCoolDownPerSecondCurve;

    //射程
    private float maxDistance;
    [SerializeField] private AnimationCurve distanceDamageFalloff;

    //扩散倍率
    private float spreadMultiplier;
    private float moveMultiplier;
    [SerializeField]private Vector2 moveSpeedRange;
    [SerializeField]private Vector2 moveMultiplierRange;
    [SerializeField] private float moveMultiplierTransSpeed;
    #endregion

    #region 生命周期
    private void Awake()
    {
        cameraShakeSource = GetComponent<CinemachineImpulseSource>();

        shootInterval = 60 / ratePerMinute;

        CalculateHeatRange();
    }
    public override void TakeOut(PlayerCharacter user)
    {
        base.TakeOut(user);

        user.leftFollow = handGuard;
        playerCamera = user.controller.playCamera.cameraComponent;

        HUDManager.GetHUD<AimHUD>()?.SetAmmo(curAmmo, fullAmmo);
    }
    private void Update()
    {
        shootTimer.TimerTick();
        coolDownTimer.TimerTick();

        UpdateSpread();

        UpdateSpreadMultiplier();

        if (shooting)
            Shooting();

        UpdateShootingParticle();
    }
    private void LateUpdate()//更新UI
    {
        float sightDis = aiming ? sightDistance : -1;
        HUDManager.GetHUD<AimHUD>()?.SetSightDis(sightDis);
    }
    public override void PutIn()
    {
        HUDManager.GetHUD<AimHUD>()?.SetAmmo(-1, -1);
    }
    #endregion

    #region 扩散
    private void CalculateHeatRange()
    {
        float min1 = heatToSpreadCurve.keys[0].time;
        float max1 = heatToSpreadCurve.keys[heatToSpreadCurve.keys.Length-1].time;

        float min2 = heatToHeatPerShotCurve.keys[0].time;
        float max2 = heatToHeatPerShotCurve.keys[heatToSpreadCurve.keys.Length - 1].time;

        float min3 = heatToCoolDownPerSecondCurve.keys[0].time;
        float max3 = heatToCoolDownPerSecondCurve.keys[heatToSpreadCurve.keys.Length - 1].time;

        minHeat = Mathf.Min(min1, Mathf.Min(min2, min3));
        maxHeat = Mathf.Min(max1, Mathf.Min(max2, max3));
    }
    private void UpdateSpread()
    {
        if(coolDownTimer<=0)
        {
            float coolDownRate = heatToCoolDownPerSecondCurve.Evaluate(heat);
            heat = ClampHeat(heat - coolDownRate * Time.deltaTime);
            spread = heatToSpreadCurve.Evaluate(heat);
        }
    }
    private void AddSpread()
    {
        float HeatPerShot = heatToHeatPerShotCurve.Evaluate(heat);
        heat = ClampHeat(heat + HeatPerShot);
        spread = heatToSpreadCurve.Evaluate(heat);
    }
    private void UpdateSpreadMultiplier()
    {
        float moveSpeed = user.cc.velocity.magnitude;
        float moveMultiplierTarget = Calc.GetMappedRangeValueClamped(moveSpeedRange, moveMultiplierRange, moveSpeed);
        moveMultiplier = Mathf.MoveTowards(moveMultiplier, moveMultiplierTarget, moveMultiplierTransSpeed * Time.deltaTime);

        spreadMultiplier = moveMultiplierTarget;
    }
    #endregion

    #region 瞄准射击
    public void SetAiming(bool value)
    {
        aiming = value;
        if (aiming)
        {
            if (heat <= onAimHeat)
            {
                spread = onAimHeat;
                coolDownTimer = coolDownTimeOnAim;
            }
        }
        else
        {
            TrySetShooting(false);
            HUDManager.GetHUD<AimHUD>()?.SetSightDis(-1);
        }
    }
    public bool TrySetShooting(bool shoot)
    {
        if (!aiming && shoot) return false;

        if (shooting)
        {
            if (curAmmo <= 0)
                return false;
            
            Shoot();
            PlayShootingParticle();

            if (automatic)
                shooting = true;
            else
                EndShootingParticle();
        }
        else
        {
            shooting = false;
            EndShootingParticle();
        }

        return true;
    }
    public void Shooting()
    {
        if (shootTimer <= 0) 
            Shoot();
        UpdateShootingParticle();
    }
    private void Shoot()
    {
        if (curAmmo <= 0)
        {
            //TODO 提示无子弹
            TrySetShooting(false);
            return;
        }
        --curAmmo;

        //瞄准的偏移
        Vector2 aimPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        aimPoint = Calc.CircleRandomPoint(aimPoint, sightDistance);

        FireBullet(aimPoint);

        AddSpread();
    }
    private void FireBullet(Vector2 aimPoint)
    {
        Ray shootRay = playerCamera.ScreenPointToRay(aimPoint);

        if (Physics.Raycast(shootRay, out RaycastHit hit, maxDistance, shootMask))
        {
            Vector3 hitDir = (hit.point - muzzle.position).normalized;
            bulletEffectVector = hitDir;

            GameObject hitObj = hit.collider.gameObject;

            //如果是这些layer就播放默认特效
            if (hitObj.layer == 0 || hitObj.layer == LayerMask.NameToLayer("Climbable"))
                PlayHitDefaultParticle(hit.point, hitDir);

            if (hitObj.layer == LayerMask.NameToLayer("EnemyPart"))
            {
                EventManager.Instance.TriggerEvent("Hit" + hitObj.GetInstanceID(),
                    new HitInfo(hitType, damage, user.gameObject, hitObj, hit.point, hitDir));
            }
        }
        else
        {
            bulletEffectVector = (playerCamera.transform.position + shootRay.direction * maxDistance - muzzle.position).normalized;
        }

        shootTimer = shootInterval;
        coolDownTimer = coolDownInterval;
    }
    #endregion

    #region 效果
    private CinemachineImpulseSource cameraShakeSource;
    private Vector3 bulletEffectVector;
    private int gunFireId, bulletTrailId;
    private void PlayShootingParticle()
    {
        gunFireId = ParticleManager.Instance.GetKeepId();
        bulletTrailId = ParticleManager.Instance.GetKeepId();
        ParticleManager.Instance.PlayEffect("GunFire", muzzle.position, 
            muzzle.rotation, keep: true, keepId: gunFireId);
        ParticleManager.Instance.PlayEffect("BulletTrail", muzzle.position, 
            Quaternion.LookRotation(bulletEffectVector), keep: true, keepId: bulletTrailId);
    }
    private void UpdateShootingParticle()
    {
        ParticleManager.Instance.SetKeep(gunFireId, muzzle.position, muzzle.rotation);
        ParticleManager.Instance.SetKeep(bulletTrailId, muzzle.position, Quaternion.LookRotation(bulletEffectVector));
    }
    private void EndShootingParticle()
    {
        ParticleManager.Instance.CloseKeep(gunFireId, true);
        ParticleManager.Instance.CloseKeep(bulletTrailId);
    }
    private void PlayHitDefaultParticle(Vector3 hitPoint,Vector3 hitDir)
    {
        Vector3 randDir = new Vector3(Random.Range(-1, 1),
            Random.Range(-1, 1), Random.Range(-1, 1));
        Vector3 particleDir = hitDir * -1 + randDir * 0.3f;
        ParticleManager.Instance.PlayEffect("BulletImpact", hitPoint, Quaternion.LookRotation(particleDir));
    }
    #endregion

    #region Tool
    private float ClampHeat(float value)
    {
        return Mathf.Clamp(value, minHeat, maxHeat);
    }
    #endregion
}
