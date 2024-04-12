using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ɫ�����õĵ��� ��ӦItem
/// </summary>
public class EquipedItem : MonoBehaviour
{
    public readonly static string rootPath = "Prefab/Item/EquipedItem/";
    [HideInInspector]public PlayerCharacter user;
    protected AbilitySystemComponent owner => user.ABS;

    public virtual void TakeOut(PlayerCharacter user,ExtraSave data)
    {
        this.user = user;
    }
    public virtual void PutIn() 
    {

    }
}
