using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObj
{
    public string prefabName;
    public bool opened { get; private set; }

    [SerializeField] private Transform objectRespawnPoint;

    public override void BeInteracted(PlayerCharacter entity)
    {
        Animation animation = GetComponentInChildren<Animation>();
        animation.Play();
        opened = true;
        TimerManager.Instance.AddTimer(new Timer(() => {
            GameObject itemPrefab = Resources.Load<GameObject>(PickableItem.prefabPath + prefabName);
            if (itemPrefab != null)
                GameObject.Instantiate(itemPrefab, objectRespawnPoint.position, Quaternion.identity);
        }, 1, 2));
    }

    public override void SetSelected(bool isSelected)
    {
        if (isSelected)
            HUDManager.GetHUD<PlayerHUD>().SetTip("Press F to Open");
        else
            HUDManager.GetHUD<PlayerHUD>().SetTip(null);
    }

    private bool playerEntered;
    public override bool CanInteract()
    {
        return !opened && playerEntered;

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
