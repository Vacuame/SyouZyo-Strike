using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ɫ�����õĵ��� ��ӦItem
/// </summary>
public class ItemInstance : MonoBehaviour
{
    Character charaBase;
    public virtual void Init(Character charaBase)
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
