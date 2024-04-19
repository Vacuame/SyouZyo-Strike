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
