using System.Xml.Serialization;
using UnityEngine.Events;

public interface IInteractable
{
    public void BeInteracted(PlayerCharacter character, UnityAction onInteractOver);

    public void SetSelected(bool isSelected);

    public bool CanInteract();

    public void EndInteract();
}
