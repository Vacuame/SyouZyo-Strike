using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ABS/Ability/Basic")]
public class AbilityAsset : ScriptableObject
{
    [Header("��������")]
    public string abilityName;

    public string[] assetTags;//���������tag
    public string[] cancelAbilityTags;//��������л�ȡ����Щ����
    public string[] blockAbilityTags;//�һ�������Щ�����������ǲ���������
    public string[] activationOwnedTag;//��������ʱ����Щ�Ӹ���ɫ
    public string[] activationRequiredTags;//��Ҫ��ɫ����ЩTag������������
    public string[] activationBlockedTags;//�ᱻ��ɫ����ЩTag��ס����������
}
