using MoleMole;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class SetSlotPanelContext : PanelContext
{
    public PlayerController playerController;
    public ItemSave itemSave;

    public SetSlotPanelContext(UIType uiType, PlayerController playerController, ItemSave itemSave):base(uiType)
    {
        this.playerController = playerController;
        this.itemSave = itemSave;
    }
}
public class SetSlotPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("Inventory/TetrisItem/SetSlotPanel");

    public Button[] btns;
    public RectTransform selectedTip;
    protected SetSlotPanelContext slotContext => context as SetSlotPanelContext;
    protected PlayerController controller => slotContext.playerController;
    protected ItemSave[] slotItems => controller.shortCutSlot;
    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);

        Debug.Log(slotItems.Length);

       for(int i=0;i<slotItems.Length;i++) 
        {
            Image img = btns[i].transform.Find("SimpleIcon").GetComponent<Image>();
            if (slotItems[i]!=null)
            {
                img.color = new Color(1, 1, 1, 1);

                EquipedItemInfo eqInfo = ItemManager.Instance.GetItemInfo(slotItems[i].id) as EquipedItemInfo;
                img.sprite = eqInfo.simpleIcon;
                RectTransform btnRect = btns[i].transform as RectTransform;
                UIExtend.FitImgSize(img, btnRect.sizeDelta.x, btnRect.sizeDelta.y);
            }
            else
            {
                img.color = new Color(1, 1, 1, 0);
            }
        }

        SetEquipTipDisplay();

        btns[0].onClick.AddListener(() => { SetSlot(0, slotContext.itemSave); });
        btns[1].onClick.AddListener(() => { SetSlot(1, slotContext.itemSave); });
        btns[2].onClick.AddListener(() => { SetSlot(2, slotContext.itemSave); });
        btns[3].onClick.AddListener(() => { SetSlot(3, slotContext.itemSave); });
    }

    public void SetSlot(int index,ItemSave itemSave)
    {
        for(int i=0; i < controller.shortCutSlot.Length; i++)
        {
            if (controller.shortCutSlot[i]==itemSave)
                controller.shortCutSlot[i] = null;
        }
        controller.shortCutSlot[index] = itemSave;
        UIManager.Instance.Pop();
    }
    protected void SetEquipTipDisplay()
    {
        bool equipInSlot = false;
        if (controller.equipingItem != null)
        {
            for (int i = 0; i < slotItems.Length; i++)
            {
                if (slotItems[i] == controller.equipingItem)
                {
                    selectedTip.transform.PanelAppearance(true);
                    selectedTip.transform.position = btns[i].transform.position;
                    equipInSlot = true;
                    break;
                }
            }
        }
        if(!equipInSlot)
        {
            selectedTip.transform.PanelAppearance(false);
        }
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            UIManager.Instance.Pop();
    }

}
