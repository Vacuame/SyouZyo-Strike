using System.Collections;
using UnityEngine;


public class GunPickableItem : PickableObj
{
    public EquipedItemSave gunSet;
    protected override ExtraSave extraSet
    {
        get => gunSet; 
        set => gunSet = value as EquipedItemSave;
    }
}
