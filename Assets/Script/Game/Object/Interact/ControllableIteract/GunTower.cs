using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTower : ControllableInteract
{
    public Transform GunTrans;
    public float rotateSpd;
    public Vector3 gunRotateOffset;

    protected override void Update()
    {
        if (!interacting)
            return;

        base.Update();

        Quaternion targetRotation = centerTransform.rotation;
        Vector3 targetEuler = targetRotation.eulerAngles + gunRotateOffset;
        targetRotation = Quaternion.Euler(targetEuler.x, targetEuler.y, targetEuler.z);

        GunTrans.rotation = Quaternion.RotateTowards(GunTrans.rotation, targetRotation, rotateSpd * Time.deltaTime);
    }
}
