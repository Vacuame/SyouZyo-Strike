using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "部位设置", menuName = "Data/部位/部位集")]
public class BodyPartSet : ScriptableObject
{
    //两种，方便设置
    public List<Pair<string, PartData>> setting;
    public List<Pair<string, SinglePartSet>> setting1;
}
[Serializable]
public struct PartData
{
    public float toughness;
    public List<Pair<HitType, float>> damageMultipler;
}

public class WeaknessData
{
    public Dictionary<HitType, float> multiplyDict;
    public WeaknessData(PartData data)
    {
        multiplyDict = new Dictionary<HitType, float>();
        foreach(var a in data.damageMultipler)
        {
            multiplyDict.Add(a.key, a.value);
        }
        //没有设置就是1倍
        foreach(HitType type in Enum.GetValues(typeof(HitType)))
        {
            if (!multiplyDict.ContainsKey(type))
                multiplyDict.Add(type, 1);
        }
    }
}

