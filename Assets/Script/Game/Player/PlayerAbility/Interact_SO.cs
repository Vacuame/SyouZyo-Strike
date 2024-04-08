using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractData", menuName = "ABS/Ability/Interact")]
public class Interact_SO : AbilityAsset
{
    [Header("��������")]
    public float interactDistance;
    [Header("��̬��")]
    [HideInInspector] public Transform centerTransform;
    [HideInInspector] public Transform cameraTransform;
}
