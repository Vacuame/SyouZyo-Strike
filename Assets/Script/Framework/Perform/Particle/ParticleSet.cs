using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "��Ч��", menuName = "�趨/��Ч")]
public class ParticleSet : ScriptableObject
{
    public List<CustomPartcle> lists;
}

[Serializable]
public struct CustomPartcle
{
    public string name;
    public ParticleSystem particle;
    public float maxLifeTime;
    public int defaultMaxNum;
}