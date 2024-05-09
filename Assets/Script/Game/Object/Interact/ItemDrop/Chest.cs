using MyUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : InteractableObj
{
    public string prefabName;
    public bool interacted { get; private set; }

    public string playerAnimName;
   
    public float openAnimStartTime;
    public float objInstanceTime;
    public float interactTime;

    private AbilityTimeLine timeline = new AbilityTimeLine();
    private PlayerCharacter player;
    private Animation anim;
    private void Awake()
    {
        anim = GetComponentInChildren<Animation>();
    }

    [SerializeField] private Transform objectRespawnPoint;
    [SerializeField] private Transform playerTransformOnOpen;

    public override void BeInteracted(PlayerCharacter character, UnityAction onInteractOver)
    {
        player = character;
        player.anim.SetLayerWeight(player.anim.GetLayerIndex("Arm"), 0);
        player.anim.Play(playerAnimName);

        timeline.AddEvent(openAnimStartTime, () => anim.Play());
        timeline.AddEvent(objInstanceTime, () =>
        {
            GameObject itemPrefab = Resources.Load<GameObject>(PickableObj.prefabPath + prefabName);
            if (itemPrefab != null)
                GameObject.Instantiate(itemPrefab, objectRespawnPoint.position, Quaternion.identity);
        });
        timeline.AddEvent(interactTime, () =>
        {
            player.anim.SetLayerWeight(player.anim.GetLayerIndex("Arm"), 1);
            onInteractOver?.Invoke();
            timeline.bPause = true;
        });
        timeline.Start();

        interacted = true;

        player.cc.enabled = false;
        TransformAlignmenter.GetOrCreateInstance()?.AddAlignRequest(new TransformAlignmenter.AlignInfo
            (player.transform, playerTransformOnOpen.position, playerTransformOnOpen.rotation, 0.5f,()=>player.cc.enabled = true));

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
