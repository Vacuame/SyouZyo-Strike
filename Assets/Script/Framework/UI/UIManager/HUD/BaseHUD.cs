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
        public virtual void OnEnter(BaseContext context)
        {
            transform.PanelAppearance(true);
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            if (!inited)
            {
                Init();
                inited = true;
            }
        }

        public virtual void OnExit(bool trueDestroy)
        {
            if (trueDestroy)
                UIManager.Instance.DestroyView(context.ViewType);
            else
                transform.PanelAppearance(false);
        }
    }
}