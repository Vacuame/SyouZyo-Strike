using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemDroper;

[CreateAssetMenu(fileName = "WaveModeInfo", menuName = "Data/GameModeInfo/Wave")]
public class WaveModeInfo : GameModeInfo
{
    [Header("�ؿ�����")]
    public int sumEnemyNum;
    public int maxEnemyNumInScene;

    [Header("�ؿ���Դ")]
    public List<DropConfig> dropConfigs;
}
