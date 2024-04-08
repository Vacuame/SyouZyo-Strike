using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 被角色手上拿的道具 对应Item
/// </summary>
public class EquipedItem : MonoBehaviour
{
    public readonly static string rootPath = "Prefab/Item/EquipedItem/";
    [HideInInspector]public Character user;
    public virtual void Init(Character charaBase)
    {
        this.user = charaBase;
    }
    public virtual void TakeOut()
    {

    }
    public virtual void PutIn() 
    {

    }
}
