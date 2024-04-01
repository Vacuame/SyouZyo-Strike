using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoleMole
{
	public class BaseView : MonoBehaviour
    {
        public virtual void OnEnter(BaseContext context)
        {
            transform.PanelAppearance(true);
            transform.SetSiblingIndex(transform.parent.childCount - 1);
        }

        //TODO 改成在这里设置透明度，而不是在DestroyView
        public virtual void OnExit(BaseContext context)
        {
            UIManager.Instance.DestroyView(context.ViewType);
        }

        public virtual void OnPause(BaseContext context)
        {
            transform.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public virtual void OnResume(BaseContext context)
        {
            transform.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
        }
	}
}
