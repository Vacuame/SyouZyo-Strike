using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractData", menuName = "ABS/Ability/Interact")]
public class Interact_SO : AbilityAsset
{
    [Header("数据设置")]
    public float interactDistance;
    [Header("动态绑定")]
    [HideInInspector] public Transform centerTransform;
    [HideInInspector] public Transform cameraTransform;
}
