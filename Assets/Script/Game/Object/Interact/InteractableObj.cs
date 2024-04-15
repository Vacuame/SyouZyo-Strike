using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObj : MonoBehaviour, IInteractable
{
    protected bool bSelected;
    public virtual bool CanInteract()
    {
        return true;
    }

    public abstract void BeInteracted(PlayerCharacter character,UnityAction onInteractOver);

    public virtual void SetSelected(bool isSelected) 
    {
        bSelected = isSelected;
    }
}
