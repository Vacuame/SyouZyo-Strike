using GameBasic;
using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllableInteract : Pawn, IInteractable
{
    protected bool bSelected;
    protected bool interacting;

    private Controller lastController;
    private Character lastCharacter;
    UnityAction interactOverAction;

    public virtual bool CanInteract()
    {
        return !interacting && playerEntered;
    }
    public virtual void SetSelected(bool isSelected)
    {
        bSelected = isSelected;
        if (isSelected)
            HUDManager.GetHUD<PlayerHUD>().SetTip("按下 'F' 操作");
        else
            HUDManager.GetHUD<PlayerHUD>().SetTip(null);
    }
    private void Update()
    {
        if (!interacting)
            return;

        if(Input.GetMouseButtonDown(1))
            ExitInteracted();
    }

    public void BeInteracted(PlayerCharacter character, UnityAction onInteractOver)
    {
        lastCharacter = character;
        lastController = character.controller;
        interactOverAction = onInteractOver;

        character.controller.ControlPawn(this);
        HUDManager.GetHUD<PlayerHUD>().SetTip("按下 '右键' 退出");
        interacting = true;
    }
    private void ExitInteracted()
    {
        interacting = false;
        lastController.ControlPawn(lastCharacter);
        interactOverAction?.Invoke();
        HUDManager.GetHUD<PlayerHUD>().SetTip(null);
    }

    private bool playerEntered;
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
            playerEntered = true;
    }
    private void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
            playerEntered = false;
    }
}
