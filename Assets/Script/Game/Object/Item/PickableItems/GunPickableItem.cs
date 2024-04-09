using System.Collections;
using UnityEngine;


public class GunPickableItem : PickableItem
{
    public GunItemSave gunSet;
    protected override ExtraSave extraSet => gunSet;
}
