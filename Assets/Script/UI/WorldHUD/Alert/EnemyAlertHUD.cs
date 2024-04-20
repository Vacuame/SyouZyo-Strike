using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertHUD : BaseHUD
{
    [SerializeField] private EnemyAlertTip tipPrefab;
    public Dictionary<GameObject,EnemyAlertTip>alertTipDict = new Dictionary<GameObject,EnemyAlertTip>();
    public void AddAlertTip(GameObject obj,Transform bindTrans)
    {
        if(!alertTipDict.ContainsKey(obj)) 
        {
            EnemyAlertTip tip = GameObject.Instantiate(tipPrefab,transform);
            tip.bind = bindTrans;
            alertTipDict.Add(obj, tip);
        }
    }
    public void RemoveAlertTip(GameObject obj) 
    {
        if (alertTipDict.ContainsKey(obj))
        {
            GameObject.Destroy(alertTipDict[obj].gameObject);
            alertTipDict.Remove(obj);
        }
    }
    public void SetAlertVisiable(GameObject obj,bool v)
    {
        if (alertTipDict.TryGetValue(obj, out EnemyAlertTip tip))
            tip.transform.PanelAppearance(v);
    }
    public EnemyAlertTip GetAlertTip(GameObject obj)
    {
        if(alertTipDict.TryGetValue(obj,out EnemyAlertTip tip))
            return tip;
        else
            return null;
    }
    private void Update()
    {
        foreach(var a in alertTipDict)
        {
            a.Value.transform.position = a.Value.bind.position;
            a.Value.transform.rotation = Camera.main.transform.rotation;
        }
    }

}

