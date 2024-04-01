using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace MoleMole
{
	public class BaseView : MonoBehaviour
    {
        public virtual void OnEnter(BaseContext context)
        {
            
        }

        public virtual void OnExit(BaseContext context)
        {

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
