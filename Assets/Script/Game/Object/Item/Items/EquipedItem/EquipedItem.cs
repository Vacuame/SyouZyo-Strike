using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ɫ�����õĵ��� ��ӦItem
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
