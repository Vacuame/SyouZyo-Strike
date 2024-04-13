using Cinemachine;
using MoleMole;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class EquipedGun : EquipedItem
{
    #region 变量
    //基本变量
    [Header("位置")]
    public Transform muzzle;
    public Transform handGuard;

    private Camera playerCamera;

    private bool shooting, aiming;

    [Header("基本设置")]
    [SerializeField] private LayerMask shootMask;
    [SerializeField] private HitType hitType;
    [SerializeField] private float damage;
    [SerializeField] private bool automatic;

    //射击冷却时间
    [Header("枪械设置")]
    [SerializeField] private float _ratePerMinute = 300;
    private float shootInterval;
    private float shootTimer;
    //扩散恢复冷却时间
    private float coolDownTimer;
    [SerializeField] private float coolDownInterval_OverUse;
    [SerializeField] private float coolDownInterval;

    //子弹
    [SerializeField] private int _fullAmmo;
    public int fullAmmo { get { return _fullAmmo; } 
        set { _fullAmmo = value; HUDManager.GetHUD<AimHUD>()?.SetAmmo(data.durability, _fullAmmo); } }
    public int curAmmo { get { return data.durability; } 
        set { data.durability = value; HUDManager.GetHUD<AimHUD>()?.SetAmmo(data.durability, _fullAmmo); } }

    // 扩散
    [Header("准星扩散")]
    private float spread;
    private float _heat;
    private float heat { get { return _heat; } set { _heat = value; spread = heatToSpreadCurve.Evaluate(_heat); } }
    private float minHeat, maxHeat;
    public float sightDistance => Screen.height * spread * spreadMultiplier / 200f;

    [SerializeField] float onAimHeat;

    [SerializeField] private AnimationCurve heatToSpreadCurve;
    [SerializeField] private AnimationCurve heatToHeatPerShotCurve;
    [SerializeField] private AnimationCurve heatToCoolDownPerSecondCurve;

    //额外扩散
    [SerializeField]private float _moveSpeedThreshold;
    private float sqrMoveSpeedThreshold;

    //扩散倍率
    private float spreadMultiplier = 1;
    /*    private float moveMultiplier;
        [SerializeField]private Vector2 moveSpeedRange;
        [SerializeField]private Vector2 moveMultiplierRange;
        [SerializeField] private float moveMultiplierTransSpeed;*/

    //射程
    [Header("距离")]
    [SerializeField] private AnimationCurve distanceDamageFalloff;
    private float maxDistance;

    #endregion

    #region Equiped
    public override void TakeOut(PlayerCharacter user, ExtraSave extra)
    {
        base.TakeOut(user, extra);
        data = extra as EquipedItemSave;
        data.equiped = true;

        user.leftFollow = handGuard;
        playerCamera = user.controller.playCamera.cameraComponent;

        user.anim.SetInteger("weaponType", 1);

        Aim_SO aimSo = Resources.Load<Aim_SO>("ScriptObjectData/Aim");
        owner.GrandAbility(new Aim(aimSo));
        AbilityAsset shootAsset = Resources.Load<AbilityAsset>("ScriptObjectData/Shoot");
        owner.GrandAbility(new Shoot(shootAsset));
        AbilityAsset reloadAsset = Resources.Load<AbilityAsset>("ScriptObjectData/Reload");
        owner.GrandAbility(new Reload(reloadAsset));

        user.controller.control.Player.Aim.started += AimSt;
        user.controller.control.Player.Aim.canceled += AimEd;
        user.controller.control.Player.Fire.started += ShootSt;
        user.controller.control.Player.Fire.canceled += ShootEd;
        user.controller.control.Player.Reload.started += Reload;

        HUDManager.GetHUD<AimHUD>()?.SetAmmo(curAmmo, fullAmmo);
    }
    public override void PutIn()
    {
        data.equiped = false;

        user.anim.SetInteger("weaponType", 0);

        user.controller.control.Player.Aim.started -= AimSt;
        user.controller.control.Player.Aim.canceled -= AimEd;
        user.controller.control.Player.Fire.started -= ShootSt;
        user.controller.control.Player.Fire.canceled -= ShootEd;
        user.controller.control.Player.Reload.started -= Reload;

        owner.RemoveAbility("Aim");
        owner.RemoveAbility("Shoot");
        owner.RemoveAbility("Reload");

        HUDManager.GetHUD<AimHUD>()?.SetAmmo(-1, -1);

        GameObject.Destroy(gameObject);
    }

    private void AimSt(CallbackContext context) =>
        owner.TryActivateAbility("Aim", user, user.chestRig, this);
    private void AimEd(CallbackContext context) =>
        owner.TryEndAbility("Aim");
    private void ShootSt(CallbackContext context) =>
            owner.TryActivateAbility("Shoot", this);
    private void ShootEd(CallbackContext context) =>
        owner.TryEndAbility("Shoot");
    private void Reload(CallbackContext context) =>
        owner.TryActivateAbility("Reload", user.anim, this);


    #endregion

    #region 生命周期
    private void Awake()
    {
        cameraShakeSource = GetComponent<CinemachineImpulseSource>();

        shootInterval = 60 / _ratePerMinute;
        sqrMoveSpeedThreshold = _moveSpeedThreshold * _moveSpeedThreshold;
        maxDistance = distanceDamageFalloff.keys.Last().time;

        CalculateHeatRange();
    }
    private void Update()
    {
        shootTimer.TimerTick();
        coolDownTimer.TimerTick();

        UpdateSpread();

        UpdateSpreadMultiplier();

        if (shooting)
        {
            Shooting();
            UpdateShootingParticle();
        }
    }
    private void LateUpdate()//更新UI
    {
        float sightDis = aiming ? sightDistance : -1;
        HUDManager.GetHUD<AimHUD>()?.SetSightDis(sightDis);
    }

    #endregion

    #region 扩散
    private void CalculateHeatRange()
    {
        float min1 = heatToSpreadCurve.keys.First().time;
        float max1 = heatToSpreadCurve.keys.Last().time;

        float min2 = heatToHeatPerShotCurve.keys.First().time;
        float max2 = heatToHeatPerShotCurve.keys.Last().time;

        float min3 = heatToCoolDownPerSecondCurve.keys.First().time;
        float max3 = heatToCoolDownPerSecondCurve.keys.Last().time;

        minHeat = Mathf.Min(min1, Mathf.Min(min2, min3));
        maxHeat = Mathf.Max(max1, Mathf.Max(max2, max3));
    }
    private void UpdateSpread()
    {
        bool moving = user.cc.velocity.sqrMagnitude > sqrMoveSpeedThreshold;
        if (moving && heat < onAimHeat)
        {
            heat = onAimHeat;
            coolDownTimer = coolDownInterval;
        }

        if (coolDownTimer <= 0)
        {
            float coolDownRate = heatToCoolDownPerSecondCurve.Evaluate(heat);
            heat = ClampHeat(heat - coolDownRate * Time.deltaTime);
        }
    }
    private void AddSpread()
    {
        float HeatPerShot = heatToHeatPerShotCurve.Evaluate(heat);
        heat = ClampHeat(heat + HeatPerShot);
    }
    private void UpdateSpreadMultiplier()
    {
/*        float moveSpeed = user.cc.velocity.magnitude;
        float moveMultiplierTarget = Calc.GetMappedRangeValueClamped(moveSpeedRange, moveMultiplierRange, moveSpeed);
        moveMultiplier = Mathf.MoveTowards(moveMultiplier, moveMultiplierTarget, moveMultiplierTransSpeed * Time.deltaTime);

        spreadMultiplier = moveMultiplierTarget;*/
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
                heat = onAimHeat;
                coolDownTimer = coolDownInterval;
            }
        }
        else
        {
            TrySetShooting(false);
            HUDManager.GetHUD<AimHUD>()?.SetSightDis(-1);
        }
    }
    public bool TrySetShooting(bool pullTriger)
    {
        if (!aiming && pullTriger) return false;

        if (pullTriger)
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

        shootTimer = shootInterval;
        coolDownTimer = coolDownInterval;

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

            EventManager.Instance.TriggerEvent("Hit" + hitObj.GetInstanceID(),
                    new HitInfo(hitType, damage, user.gameObject, hitObj, hit.point, hitDir));
        }
        else
        {
            bulletEffectVector = (playerCamera.transform.position + shootRay.direction * maxDistance - muzzle.position).normalized;
        }

        Vector3 shakeVector = cameraShakeSource.m_DefaultVelocity;
        shakeVector.x = Random.Range(-0.1f, 0.1f);
        cameraShakeSource.GenerateImpulse(shakeVector * 0.1f);
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
