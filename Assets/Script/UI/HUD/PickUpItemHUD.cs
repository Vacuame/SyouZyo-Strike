using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemHUD : WorldHUD
{
    [SerializeField] private GameObject view;
    public void SetPickUpTip(Vector3 pos)
    {
        view.SetActive(true);
        view .transform.position = pos;
    }
    public void HidePickupTip()
    {
        view.SetActive(false);
    }
}
