using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : PickableObj
{
    [SerializeField]private ExtraSave extra;
    public override ExtraSave extraSet { get => extra; set => extra = value; }
}
