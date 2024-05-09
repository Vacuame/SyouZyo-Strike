using MyUI;
using UnityEngine;
using UnityEngine.UI;

public class GunTowerHUD : BaseHUD
{
    public Text textTip;
    public Slider slider;
    string tipText;
    protected override void Init()
    {
        tipText = textTip.text;
    }
    public void SetReload(float proportion)
    {
        proportion = Mathf.Clamp01(proportion);
        slider.value = proportion;
        if (proportion == 1f)
            textTip.text = tipText;
        else
            textTip.text = "--×°ÌîÖÐ--";
    }
}
