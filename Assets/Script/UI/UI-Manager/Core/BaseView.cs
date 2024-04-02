using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;

namespace MoleMole
{
	public class BaseView : MonoBehaviour
    {
        protected BaseContext context;
        public virtual void OnEnter(BaseContext context)
        {
            this.context = context;
            transform.PanelAppearance(true);
            transform.SetSiblingIndex(transform.parent.childCount - 1);
        }

        public virtual void OnExit(bool trueDestroy)
        {
            if (trueDestroy)
                UIManager.Instance.DestroyView(context.ViewType);
            else
                transform.PanelAppearance(false);
        }

        public virtual void OnPause()
        {
            transform.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public virtual void OnResume()
        {
            transform.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
