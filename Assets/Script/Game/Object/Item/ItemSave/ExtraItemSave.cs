using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtraSave
{
    public ItemSave itemSave;
    [SerializeField]private int _num = 1;
    public int num { get { return _num; }
        set { _num = value; onNumChanged?.Invoke(_num); }
    }
    public Action<int> onNumChanged;

    public ExtraSave(int num) 
    {
        onNumChanged += (int num) => 
        {
            if(num <= 0)
                itemSave.RemoveSelf();
        };
        this.num = num;
    }
}

[System.Serializable]
public class GunItemSave : ExtraSave
{
    public int curAmmo;
    private bool _equiped;
    public bool equiped { get { return _equiped; }
        set { _equiped = value; onEquipedChanged?.Invoke(_equiped); }
    }
    public Action<bool> onEquipedChanged;
    public GunItemSave(int curAmmo,bool equiped,int num = 1):base(num)
    {
        this.curAmmo = curAmmo;
        this._equiped = equiped;
    }
}