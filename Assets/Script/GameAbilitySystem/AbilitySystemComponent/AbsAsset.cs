using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "ABS/ABS/AbsAsset")]
public class AbsAsset : ScriptableObject
{
    public List<Pair<string, string>> AttributeSetting;
}
