using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtraSave
{

}

[System.Serializable]
public class GunItemSave : ExtraSave
{
    public int curAmmo;
    private bool _equiped;
    public bool equiped { get { return _equiped; }
        set { _equiped = value; onEquipedChange?.Invoke(_equiped); }
    }
    public Action<bool> onEquipedChange;
    public GunItemSave(int curAmmo,bool equiped)
    {
        this.curAmmo = curAmmo;
        this._equiped = equiped;
    }
}