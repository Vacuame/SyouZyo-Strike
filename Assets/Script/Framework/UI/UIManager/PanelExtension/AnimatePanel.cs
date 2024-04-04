using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MoleMole
{
    [RequireComponent(typeof(Animator))]
	public abstract class AnimatePanel : BasePanel 
    {
        [SerializeField]
        protected Animator _animator;

        public override void OnEnter(PanelContext context)
        {
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            _animator.SetTrigger("OnEnter");
        }

        public override void OnExit(bool trueDestroy)
        {
            _animator.SetTrigger("OnExit");
        }

        public override void OnPause()
        {
            base.OnPause();
            _animator.SetTrigger("OnPause");
        }

        public override void OnResume()
        {
            base.OnResume();
            _animator.SetTrigger("OnResume");
        }

	}
}
