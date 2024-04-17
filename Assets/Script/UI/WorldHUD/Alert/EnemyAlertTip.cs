using MoleMole;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAlertTip : MonoBehaviour
{
    [SerializeField] private Image img;
    private RectTransform rect => img.transform as RectTransform;
    [SerializeField] private Color alertColor, findColor;//»Æ¡¢ºì
    [HideInInspector]public Transform bind;
    public void SetValue(float value)
    {
        //pivot 1.5Îª¿Õ   0.5ÎªÂú
        float pivot = 1.5f - value;
        rect.pivot = new Vector2 (0.5f, pivot);

        Color c;
        if(value<=0.5f)
        {
            c = Color.Lerp(Color.white, alertColor, value * 2);
        }
        else
        {
            c = Color.Lerp(alertColor, findColor, (value - 0.5f) * 2 );
        }
        img.color = c;
    }
    public void SetVisiable(bool vis)
    {
        transform.PanelAppearance(vis);
    }
}
