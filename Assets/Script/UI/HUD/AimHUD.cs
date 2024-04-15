using MoleMole;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimHUD : BaseHUD
{
    [SerializeField] private RectTransform sight;
    [SerializeField]private List<Pair<string, TextMeshProUGUI>> _texts=new List<Pair<string, TextMeshProUGUI>>();
    private Dictionary<string, TextMeshProUGUI> texts=new Dictionary<string, TextMeshProUGUI>();
    List<RectTransform> sightSides=new List<RectTransform>();
    static readonly Vector2[] sightSideDir = {new Vector2(1,0),new Vector2(0,1),new Vector2(-1,0),new Vector2(0,-1) };
    protected override void Init()
    {
        foreach (var a in _texts)
            texts.Add(a.key, a.value);

        foreach(var s in sight.GetComponentsInChildren<RectTransform>())
        {
            if (s.transform==sight) continue;
            sightSides.Add(s);
        }
    }

    public void SetText(string key,string value)
    {
        if(texts.TryGetValue(key,out TextMeshProUGUI text))
        {
            if (text == null) return;
            if (value == null)
                value = "";
            text.text = value;
        }
    }

    public void SetBagAmmo(float bag)
    {
        SetText("bagAmmo", bag.ToString());
        if (texts.TryGetValue("bagAmmo", out TextMeshProUGUI text))
        {
            Color color = Color.white;
            if (bag == 0)
                color = Color.red;
            text.color = color;
        }
    }

    public void SetCurAmmo(float cur, float full)
    {
        SetText("curAmmo", cur.ToString());
        if (texts.TryGetValue("curAmmo", out TextMeshProUGUI text))
        {
            Color color = Color.white;
            if (cur == full)
                color = Color.green;
            else if (cur == 0)
                color = Color.red;
            text.color = color;
        }
    }

    public void SetAmmo(float cur,float full,float bag)
    {
        if(cur<0 || full<0 || full < 0)
        {
            SetText("curAmmo", null);
            SetText("ammoCenterLine", null);
            SetText("bagAmmo", null);
        }
        else
        {
            SetCurAmmo(cur, full);
            SetText("ammoCenterLine", "/");
            SetBagAmmo(bag);
        }
    }
    public void SetSightDis(float distance)
    {
        if (sight == null) return;
        if (distance < 0)
        {
            sight.gameObject.SetActive(false);
            return;
        }
        sight.gameObject.SetActive(true);
        for(int i = 0; i < sightSides.Count; i++)
            sightSides[i].localPosition = sightSideDir[i]*distance;
    }

}
