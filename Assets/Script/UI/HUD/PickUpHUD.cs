using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoleMole;
using UnityEngine.UI;

public class PickUpHUD : BaseHUD
{
    [SerializeField] private Transform pickUpTip;
    [SerializeField] private Text txt_itemName,txt_itemNum;
    [SerializeField] private Vector2 tipPosOffset;

    private Transform followingTransform;

    public void SetPickUpTip(Transform itemTrans,string itemName,int itemNum)
    {
        SetVisiable(true);
        followingTransform = itemTrans;
        txt_itemName.text = itemName;
        txt_itemNum.text = $"¡Á {itemNum}";
        //pickUpTip.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        LayoutRebuilder.ForceRebuildLayoutImmediate(pickUpTip.transform as RectTransform);
    }

    private void Update()
    {
        if(followingTransform != null) 
        {
            Vector3 uiPos = Camera.main.WorldToScreenPoint(followingTransform.position);
            uiPos += (Vector3)tipPosOffset;
            pickUpTip.position = uiPos;
        }
    }

    public void HidePickUpTip()
    {
        SetVisiable(false);
        followingTransform = null;
    }

}
