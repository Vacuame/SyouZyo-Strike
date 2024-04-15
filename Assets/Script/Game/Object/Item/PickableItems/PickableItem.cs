using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : PickableObj
{
    public ExtraSave extra;
    protected override ExtraSave extraSet { get => extra; set => extra = value; }
}
