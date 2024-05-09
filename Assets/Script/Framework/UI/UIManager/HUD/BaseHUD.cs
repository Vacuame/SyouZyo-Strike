using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace MyUI
{
    public abstract class BaseHUD : MonoBehaviour
    {
        protected bool inited;

        protected virtual void Init()
        {
            
        }

        public virtual void OnEnter()
        {
            transform.PanelAppearance(true);
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            if (!inited)
            {
                Init();
                inited = true;
            }
        }

        public virtual void SetVisiable(bool visiable)
        {
            transform.PanelAppearance(visiable);
        }
    }
}