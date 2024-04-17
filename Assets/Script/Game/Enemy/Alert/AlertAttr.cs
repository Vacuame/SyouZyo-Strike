using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertAttr : AttributeSet
{
    public AttributeBase alert;
    public AttributeBase searchTime;
    public AlertAttr(AlertAttrAsset asset)
    {
        alert = new AttributeBase(SetName,"alert",asset.alertToFind);
        alert.SetCurValue(0);
        searchTime = new AttributeBase(SetName, "searchTime", asset.searchTime);
        searchTime.SetCurValue(0);
    }

    public override AttributeBase this[string key]
    {
        get
        {
            switch (key)
            {
                case "alert": return alert;
                case "searchTime":return searchTime;
            }
            return null;
        }
    }

    public override string[] AttributeNames => new string[] {"alert", "searchTime" };
}
