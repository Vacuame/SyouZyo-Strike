using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace MoleMole
{
    public class BaseHUD : MonoBehaviour
    {
        protected BaseContext context;
        protected bool inited;
        protected virtual void Init()
        {
            
        }

        public virtual void OnInstance(BaseContext context)
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