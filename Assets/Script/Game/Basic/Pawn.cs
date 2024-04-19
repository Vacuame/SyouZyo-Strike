using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace GameBasic
{
    /// <summary>
    /// ���Զ��Ķ��������Ա��󶨿�����
    /// </summary>
    public class Pawn : MonoBehaviour
    {
        [SerializeField, Header("�� as Pawn")]
        public Transform centerTransform;

        [HideInInspector] public bool bControlable = true;
        [HideInInspector] public Controller controller { get; private set; }

        protected virtual void Awake()
        {
            if (centerTransform == null)
                centerTransform = transform;
            EventManager.Instance.AddFunc("GetCenterTrans" + gameObject.GetInstanceID(),
                new Func<Transform>(() => centerTransform));
        }

        public virtual void SetController(Controller controller)
        {
            this.controller = controller;
            //�������дһЩ��
        }

        public virtual void RemoveController()
        {
            this.controller = null;
            //����Ҫ�����
        }

    }

}
