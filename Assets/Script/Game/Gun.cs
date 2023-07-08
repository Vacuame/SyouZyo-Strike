using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle, handGuard;
    [SerializeField] private float rateOfGun = 300;
    private bool shooting,aiming;
    private float shootInterval;
    private float timeToNextShoot;
    [SerializeField] LayerMask shootMask;

    [SerializeField] private int _curAmmo,_fullAmmo;
    public int curAmmo { get { return _curAmmo; }set{ _curAmmo = value; GameUI.Instance?.SetAmmo(_curAmmo, _fullAmmo); } }
    public int fullAmmo { get { return _fullAmmo; }set{ _fullAmmo = value; GameUI.Instance?.SetAmmo(_curAmmo, _fullAmmo); } }

    /// <summary>扩散的屏幕百分比</summary>
    [Header("扩散")]
    [SerializeField] private float spread,moveSpread;
    public bool moving;
    [SerializeField] private float onAimSpread;
    [SerializeField] private float onMoveSpread,moveSpreadShrinkSpeed;
    [SerializeField] private float minSpread,maxSpread;
    private float baseSpread;
    [SerializeField] private float minSpreadShrinkSpeed, maxSpreadShrinkSpeed, spreadShrinkAcc;
    private float spreadShrinkSpeed;
    [SerializeField] private float aimShrinkWait;
    [SerializeField] private float overUsedShrinkWait;
    private float shrinkWaitTimer;

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
    }

    private void OnEnable()
    {
        GameUI.Instance?.SetAmmo(curAmmo, fullAmmo);
    }
    private void OnDisable()
    {
        SetAiming(false);
        GameUI.Instance?.SetAmmo(-1, -1);
    }

    private void Update()
    {
        moveSpread = Mathf.MoveTowards(moveSpread, 0, moveSpreadShrinkSpeed);
        if (moving) moveSpread = onMoveSpread;

        if (aiming)
        {
            sightDistance = Screen.height * (spread + moveSpread) / 200;

            if (shooting)
            {
                if (timeToNextShoot.TimePassBy() <= 0) Shoot();
                ParticleManager.Instance.SetKeep(gunFireId, muzzle.position, muzzle.rotation);
                ParticleManager.Instance.SetKeep(bulletTrailId, muzzle.position, Quaternion.LookRotation(bulletVector));
            }
        }

        //不射击缩小准星 或者 准星扩散大于缩小时才可以缩小（否则准星无法增大）
        if(shrinkWaitTimer.TimePassBy()<=0)
            if (!shooting||lastSpreadGrow > spreadShrinkSpeed * shootInterval)
            {
                spread = Mathf.MoveTowards(spread, minSpread, spreadShrinkSpeed * Time.deltaTime);
                spreadShrinkSpeed = Mathf.MoveTowards(spreadShrinkSpeed, maxSpreadShrinkSpeed, spreadShrinkAcc * Time.deltaTime);
            }
    }

    private void LateUpdate()//更新UI
    {
        float sightDis = aiming ? sightDistance : -1;
        GameUI.Instance.SetSightDis(sightDis);   
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
            GameUI.Instance.SetSightDis(-1);
        }
    }

    public bool TrySetShooting(bool value)
    {
        if (!aiming) return false;

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
            ParticleManager.Instance.PlayEffect("BulletImpact" ,hit.point, Quaternion.LookRotation(dir));
        }
    }
}
