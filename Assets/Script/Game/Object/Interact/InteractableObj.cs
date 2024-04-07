using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObj : MonoBehaviour, IInteractable
{
    protected bool bSelected;
    public virtual bool canInteract => true;

    public abstract void BeInteracted(PlayerCharacter character);

    public virtual void SetSelected(bool isSelected) 
    {
        bSelected = isSelected;
    }
}
