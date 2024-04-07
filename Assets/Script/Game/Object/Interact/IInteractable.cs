using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void BeInteracted(PlayerCharacter character);

    public void SetSelected(bool isSelected);

    public bool canInteract { get; }
}
