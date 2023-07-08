using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "特效集", menuName = "设定/特效")]
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