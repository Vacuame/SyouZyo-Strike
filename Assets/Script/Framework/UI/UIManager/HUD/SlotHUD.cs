using MyUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotHUD : BaseHUD
{
    public Image[] cells;
    public RectTransform selectedTip;
    [SerializeField] private float timeToHide;
    private float hideTimer;

    private CanvasGroup cg;

    private void Awake()
    {
        cg = transform.GetOrAddComponent<CanvasGroup>();
    }

    public void DisplayEquipSlot(ItemSave[] slotItems,ItemSave equipingItem)
    {
        hideTimer = timeToHide;
        bool equipInSlot = false;
        for (int i = 0; i < slotItems.Length; i++)
        {
            Image img = cells[i].transform.Find("SimpleIcon").GetComponent<Image>();
            if (slotItems[i] != null)
            {
                img.color = new Color(1, 1, 1, 1);

                EquipedItemInfo eqInfo = ItemManager.Instance.GetItemInfo(slotItems[i].id) as EquipedItemInfo;
                img.sprite = eqInfo.simpleIcon;
                RectTransform cellRect = cells[i].transform as RectTransform;
                UIExtend.FitImgSize(img, cellRect.sizeDelta.x, cellRect.sizeDelta.y);

                if (slotItems[i] == equipingItem)
                {
                    selectedTip.transform.position = cells[i].transform.position;
                    selectedTip.transform.PanelAppearance(true);
                    equipInSlot = true;
                }
            }
            else
            {
                img.color = new Color(1, 1, 1, 0);
            }
        }

        if (!equipInSlot)
        {
            selectedTip.transform.PanelAppearance(false);
        }
    }

    private void Update()
    {
        if(hideTimer>0)
        {
            hideTimer.TimerTick();
            cg.alpha = Mathf.Lerp(0, 1, hideTimer / timeToHide);
        }
    }
}
