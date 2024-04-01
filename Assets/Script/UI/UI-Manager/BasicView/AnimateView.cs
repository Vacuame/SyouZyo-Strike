using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MoleMole
{
    [RequireComponent(typeof(Animator))]
	public abstract class AnimateView : BaseView 
    {
        [SerializeField]
        protected Animator _animator;

        public override void OnEnter(BaseContext context)
        {
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            _animator.SetTrigger("OnEnter");
        }

        public override void OnExit(BaseContext context)
        {
            _animator.SetTrigger("OnExit");
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
            _animator.SetTrigger("OnPause");
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
            _animator.SetTrigger("OnResume");
        }

	}
}
