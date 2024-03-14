using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ɫ�����õĵ��� ��ӦItem
/// </summary>
public class ItemInstance : MonoBehaviour
{
    CharacterBase charaBase;
    public virtual void Init(CharacterBase charaBase)
    {
        this.charaBase = charaBase;
    }
    public virtual void TakeOut()
    {

    }
    public virtual void PutIn() 
    {

    }
}
