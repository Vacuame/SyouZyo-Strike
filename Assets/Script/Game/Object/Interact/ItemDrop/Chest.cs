using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : InteractableObj
{
    public string prefabName;
    public bool interacted { get; private set; }

    public string playerAnimName;
   
    public float interactTime;
    public float openAnimStartTime;
    public float objInstanceTime;

    private AbilityTimeLine timeline = new AbilityTimeLine();
    private PlayerCharacter player;
    private Animation anim;
    private void Awake()
    {
        anim = GetComponentInChildren<Animation>();
    }

    [SerializeField] private Transform objectRespawnPoint;

    public override void BeInteracted(PlayerCharacter character, UnityAction onInteractOver)
    {
        player = character;
        player.anim.Play(playerAnimName);

        timeline.AddEvent(interactTime, () =>
        {
            onInteractOver?.Invoke();
            timeline.bPause = true;
        });
        timeline.AddEvent(openAnimStartTime, () => anim.Play());
        timeline.AddEvent(objInstanceTime, () =>
        {
            GameObject itemPrefab = Resources.Load<GameObject>(PickableObj.prefabPath + prefabName);
            if (itemPrefab != null)
                GameObject.Instantiate(itemPrefab, objectRespawnPoint.position, Quaternion.identity);
        });
        timeline.Start();

        interacted = true;

        HUDManager.GetHUD<PlayerHUD>().SetTip(null);
    }

    private void Update()
    {
        timeline.Update();
    }

    public override void SetSelected(bool isSelected)
    {
        base.SetSelected(isSelected);
        if (isSelected)
            HUDManager.GetHUD<PlayerHUD>().SetTip("Press F to Open");
        else
            HUDManager.GetHUD<PlayerHUD>().SetTip(null);
    }

    private bool playerEntered;
    public override bool CanInteract()
    {
        return !interacted && playerEntered;

    }
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
