using GameBasic;
using MyUI;
using UnityEngine;
using UnityEngine.Events;

public abstract class ControllableInteract : Pawn, IInteractable
{
    protected bool bSelected;
    protected bool interacting;

    private Controller lastController;
    private Character lastCharacter;
    UnityAction interactOverAction;

    [SerializeField, Header("相机角度限制")]
    public float camTopClamp;
    public float camBottomClamp;
    public float camYawClamp;
    public float exCamPitch;
    public float exCamYaw;

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
    protected virtual void Update()
    {
        if (!interacting)
            return;

        if(Input.GetMouseButtonDown(1))
            EndInteract();
    }

    public virtual void BeInteracted(PlayerCharacter character, UnityAction onInteractOver)
    {
        lastCharacter = character;
        lastController = character.controller;
        interactOverAction = onInteractOver;

        character.controller.ControlPawn(this);
        interacting = true;
    }
    public virtual void EndInteract()
    {
        if (interacting)
        {
            interacting = false;
            lastController.ControlPawn(lastCharacter);
            interactOverAction?.Invoke();
        }
    }

    public override void SetController(Controller controller)
    {
        controller.playCamera.SwitchCamera(Tags.Camera.Interact);
        controller.SetRotateLimit(camTopClamp,camBottomClamp, camYawClamp,exCamPitch,exCamYaw);
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
