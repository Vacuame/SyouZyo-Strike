using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "��λ����", menuName = "Data/��λ/��λ��")]
public class BodyPartSet : ScriptableObject
{
    //���֣���������
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
        //û�����þ���1��
        foreach(HitType type in Enum.GetValues(typeof(HitType)))
        {
            if (!multiplyDict.ContainsKey(type))
                multiplyDict.Add(type, 1);
        }
    }
}

