using System.Collections;
using UnityEngine;


public class GunPickableItem : PickableItem
{
    public EquipedItemSave gunSet;
    protected override ExtraSave extraSet => gunSet;
}
