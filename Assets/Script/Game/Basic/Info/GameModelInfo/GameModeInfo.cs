using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeInfo", menuName = "Data/GameModeInfo/GameMode")]
public class GameModeInfo : ScriptableObject
{
    [Header("�ؿ�����")]
    public float playerInjuryMultiplier;
    public float enemyInjuryMultiplier;
}

