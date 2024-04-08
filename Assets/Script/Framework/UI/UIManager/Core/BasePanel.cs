using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;

namespace MoleMole
{
	public class BasePanel : MonoBehaviour
    {
        protected PanelContext context;
        protected bool inited;
        protected virtual void Init()
        {

        }
        public virtual void OnEnter(PanelContext context)
        {
            this.context = context;
            transform.PanelAppearance(true);
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            if(!inited) 
            {
                Init();
                inited = true;
            }
        }

        public virtual void OnExit(bool trueDestroy)
        {
            if (trueDestroy)
                UIManager.Instance.DestroyView(context.uiType);
            else
                transform.PanelAppearance(false);
        }

        public virtual void OnPause()
        {
            transform.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
            enabled = false;
        }

        public virtual void OnResume()
        {
            transform.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
            enabled = true;
        }
    }
}
