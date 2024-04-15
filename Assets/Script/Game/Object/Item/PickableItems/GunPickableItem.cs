using System.Collections;
using UnityEngine;


public class GunPickableItem : PickableObj
{
    [SerializeField]private EquipedItemSave gunSet;
    public override ExtraSave extraSet
    {
        get => gunSet; 
        set => gunSet = value as EquipedItemSave;
    }
}
