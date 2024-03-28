using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAttr : AttributeSet
{
    private Dictionary<string, AttributeBase> partToughDict = new Dictionary<string, AttributeBase>();
    public BodyAttr(BodyPartSet bodyPartSet)
    {
        foreach (var a in bodyPartSet.setting)
            partToughDict.Add(a.key, new AttributeBase(SetName, a.key, a.value.toughness));
    }
    public override AttributeBase this[string key] => partToughDict[key];

    public override string[] AttributeNames
    {
        get
        {
            string[] strings = new string[partToughDict.Count];
            int i = 0;
            foreach (var a in partToughDict)
                strings[i++] = a.Key;
            return strings;
        }
    }
}
