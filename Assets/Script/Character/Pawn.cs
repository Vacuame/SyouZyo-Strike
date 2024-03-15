using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Pawn : MonoBehaviour
{
    [SerializeField, Header("°ó¶¨")]
    public Transform camTraceTransform;

    [HideInInspector] public bool bControlable = true;
    [HideInInspector] public Controller controller;

    protected virtual void Awake()
    {
        if(camTraceTransform == null)
            camTraceTransform = transform;
    }


}
