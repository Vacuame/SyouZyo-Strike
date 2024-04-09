using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : AbstractAbility<Interact_SO>
{
    public Interact(AbilityAsset setAsset) : base(setAsset)
    {

    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new InteractSpec(this,owner);
    }

    public class InteractSpec : AbilitySpec
    {
        private Interact_SO asset => interact.AbilityAsset;
        private Interact interact;

        [HideInInspector] public List<Transform> interactableObjs = new List<Transform>();

        //private Transform selectedInteractTarget;
        private IInteractable selectedInteractable;

        //private float noInterTimer;
        public InteractSpec(AbstractAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            interact = ability as Interact;
        }

        public override void ActivateAbility(params object[] args)
        {
            selectedInteractable?.BeInteracted(args[0] as PlayerCharacter);

            EndSelf();
        }

        public override void EndAbility()
        {
            
        }

        /// <summary>
        /// �����������
        /// </summary>
        protected override void SustainedTick()
        {
            Transform centerTrans = asset.centerTransform;
            Transform camTrans = asset.cameraTransform;
            float interactDis = asset.interactDistance;
            Collider[] preCheck = Physics.OverlapSphere(centerTrans.position, interactDis, LayerMask.GetMask("Interactable"/*, "Enemy"*/));
            if (TrySelectBestInteract(ref preCheck, camTrans.position, camTrans.forward, out Transform interactTarget, out IInteractable interactable))
            {
                if (interactable != selectedInteractable)
                {
                    /*                    if (Calc.TryGetInterfaceInTransform(wasInteractTarget, out IInteractable wasInteraction))
                                            wasInteraction.SetSelected(false);*/
                    selectedInteractable?.SetSelected(false);
                    interactable.SetSelected(true);
                    selectedInteractable = interactable;
                }
                //selectedInteractTarget = interactTarget;
            }
            else
            {
/*                if (Calc.TryGetInterfaceInTransform(selectedInteractTarget, out IInteractable wasInteraction))
                    wasInteraction.SetSelected(false);*/
                selectedInteractable?.SetSelected(false);
                selectedInteractable = null;
            }
        }

        private bool TrySelectBestInteract(ref Collider[] list, Vector3 from, Vector3 forward, out Transform resTrans, out IInteractable resInter)
        {
            forward = forward.normalized;
            resTrans = null;
            resInter = null;
            float bestDot = -1;
            for (int i = 0; i < list.Length; i++)
            {
                var a = list[i].transform;
/*                if (a == null)
                {
                    Debug.LogError(a + "is null but in list");
                    continue;
                }*/
                if (Calc.TryGetInterfaceInTransform(a, out IInteractable interaction) && interaction.canInteract)
                {
                    Vector3 vector = (a.position - from).normalized;
                    float dot = Vector3.Dot(vector, forward);
                    if (dot > bestDot && dot > 0)
                    {
                        bestDot = dot;
                        resTrans = a;
                        resInter = interaction;
                    }
                }
            }
            return resTrans != null;
        }
    }
}