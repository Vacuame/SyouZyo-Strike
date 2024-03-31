using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameUI : SingletonMono<GameUI>
{
    [SerializeField] private RectTransform sight;
    [SerializeField]private List<Pair<string, TextMeshProUGUI>> _texts=new List<Pair<string, TextMeshProUGUI>>();
    private Dictionary<string, TextMeshProUGUI> texts=new Dictionary<string, TextMeshProUGUI>();
    List<RectTransform> sightSides=new List<RectTransform>();
    static readonly Vector2[] sightSideDir = {new Vector2(1,0),new Vector2(0,1),new Vector2(-1,0),new Vector2(0,-1) };
    protected override void Awake()
    {
        base.Awake();
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
            if(value==null)
            {
                text.gameObject.SetActive(false);
                return;
            }
            text.gameObject.SetActive(true);
            text.text = value;
        }

    }

    public void SetAmmo(float now,float full)
    {
        if(now<0&&full<0)
        {
            SetText("ammo", null);
            return;
        }
        SetText("ammo", $"{now} / {full}");
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
