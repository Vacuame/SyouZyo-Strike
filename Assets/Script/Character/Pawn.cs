using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Pawn : MonoBehaviour
{
    [SerializeField, Header("绑定")]
    public Transform camTraceTransform;

    [HideInInspector] public bool bControlable = true;
    [HideInInspector] public Controller controller { get; private set; }

    protected virtual void Awake()
    {
        if(camTraceTransform == null)
            camTraceTransform = transform;
    }

    public virtual void SetController(Controller controller)
    {
        this.controller = controller;
        //子类可以写一些绑定
    }

    public virtual void RemoveController() 
    { 
        this.controller = null;
        //子类要解除绑定
    }

}
