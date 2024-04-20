using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunTower : ControllableInteract
{
    public Transform GunTrans;
    public float rotateSpd;
    public Vector3 gunRotateOffset;
    public RayView laser;
    [Header("Missile")]
    [SerializeField] private Missile missile;
    [SerializeField] private float missileSpd;
    private Vector3 missileOriginLocalPos;
    [Header("Reload")]
    [SerializeField]
    private float reloadTime;
    private float reloadTimer;

    protected override void Awake()
    {
        base.Awake();
        missileOriginLocalPos = missile.transform.localPosition;
    }

    protected override void Update()
    {
        if (!interacting)
            return;

        base.Update();

        Quaternion gunTargetRotation = centerTransform.rotation;
        Vector3 targetEuler = gunTargetRotation.eulerAngles + gunRotateOffset;
        gunTargetRotation = Quaternion.Euler(targetEuler.x, targetEuler.y, targetEuler.z);

        GunTrans.rotation = Quaternion.RotateTowards(GunTrans.rotation, gunTargetRotation, rotateSpd * Time.deltaTime);

        if(Input.GetMouseButtonDown(0))
        {
            if(!missile.lauched)
            {
                missile.Launch(laser.transform.forward, missileSpd);
                reloadTimer = reloadTime;
            }
        }
        if(missile.lauched)
        {
            reloadTimer.TimerTick();
            if(reloadTimer<=0)
            {
                missile.Init(GunTrans, missileOriginLocalPos);
            }
        }
    }

    public override void BeInteracted(PlayerCharacter character, UnityAction onInteractOver)
    {
        base.BeInteracted(character, onInteractOver);
        laser.enabled = true;
    }
    public override void EndInteract()
    {
        base.EndInteract();
        laser.enabled = false;
    }

}
