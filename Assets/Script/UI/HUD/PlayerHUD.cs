using MyUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : BaseHUD
{
    [SerializeField] private Slider hpSlider;
    [HideInInspector]private Image hpSlider_Img;
    [SerializeField] private List<Pair<float,Color>> hpColorSetting;
    [SerializeField] private Text txtTip;
    protected override void Init()
    {
        hpSlider_Img = hpSlider.transform.Find("Fill Area").GetComponentInChildren<Image>();
    }
    public void SetHpValue(float value)
    {
        if (value <= 0)
        {
            hpSlider_Img.color = new Color(0, 0, 0, 0);
            return;
        }
        hpSlider.value = value;
        Color color = hpColorSetting.Find(a=> value * 100f <= a.key).value;
        hpSlider_Img.color = color;
    }

    private float tipTimer;
    public void SetTip(string str)
    {
        if (str == null)
            str = "";
        txtTip.text = str;
    }

}
