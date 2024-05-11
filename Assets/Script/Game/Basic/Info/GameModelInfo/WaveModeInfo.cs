using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemDroper;

[CreateAssetMenu(fileName = "WaveModeInfo", menuName = "Data/GameModeInfo/Wave")]
public class WaveModeInfo : GameModeInfo
{
    [Header("关卡配置")]
    public int sumEnemyNum;
    public int maxEnemyNumInScene;

    [Header("关卡资源")]
    public List<DropConfig> dropConfigs;
}
