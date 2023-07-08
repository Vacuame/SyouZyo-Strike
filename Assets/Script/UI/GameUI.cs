using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : SingletonMono<GameUI>
{
    [SerializeField] private RectTransform sight;
    [SerializeField] private TextMeshProUGUI ammo;
    List<RectTransform> sightSides=new List<RectTransform>();
    static readonly Vector2[] sightSideDir = {new Vector2(1,0),new Vector2(0,1),new Vector2(-1,0),new Vector2(0,-1) };
    protected override void Awake()
    {
        base.Awake();
        foreach(var s in sight.GetComponentsInChildren<RectTransform>())
        {
            if (s.transform==sight) continue;
            sightSides.Add(s);
        }
    }

    public void SetAmmo(float now,float full)
    {
        if (ammo == null) return;
        if(now<0&&full<0)
        {
            ammo.gameObject.SetActive(false);
            return;
        }
        ammo.gameObject.SetActive(true);
        string ammoStr = $"{now} / {full}";
        ammo.text = ammoStr;
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
