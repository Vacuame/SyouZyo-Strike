using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GunItemSave : ItemSave
{
    public int curAmmo;
    public bool _equiped;
    public bool equiped { get { return _equiped; }
        set { _equiped = value; onEquipedChange?.Invoke(_equiped); }
    }
    public Action<bool> onEquipedChange;
    public GunItemSave(int id, Vector2Int pos, InventoryStatic.Dir dir,int curAmmo,bool equiped) : base(id, pos, dir)
    {
        this.curAmmo = curAmmo;
        this._equiped = equiped;
    }
}