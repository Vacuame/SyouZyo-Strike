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
        set 
        { 
            _num = value; 
            onNumChanged?.Invoke(_num);
            if (_num <= 0)
            {
                itemSave.RemoveSelf();
            }
        }
    }
    public Action<int> onNumChanged;

    public ExtraSave(int num) 
    {
        this.num = num;
    }
}

[System.Serializable]
public class EquipedItemSave : ExtraSave
{
    public int durability;
    private bool _equiped;
    public bool equiped { get { return _equiped; }
        set { _equiped = value; onEquipedChanged?.Invoke(_equiped); }
    }
    public Action<bool> onEquipedChanged;
    public EquipedItemSave(int durability, bool equiped,int num = 1):base(num)
    {
        this.durability = durability;
        this._equiped = equiped;
    }
}