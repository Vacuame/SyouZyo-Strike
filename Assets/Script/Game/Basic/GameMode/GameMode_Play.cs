using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Play : GameMode
{
    protected List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector] public PlayerController playerController;

    [HideInInspector]public GameModeInfo levelInfo;
    public float playerInjuryMultiplier;
    public float enemyInjuryMultiplier;

    public virtual void OnEnemySpawn(Enemy e)
    {
        enemyList.Add(e);
    }
    public virtual void OnEnemyDead(Enemy e)
    {
        enemyList.Remove(e);
    }

    public virtual void OnNotifyPlayer(GameObject player)
    {

    }

    public virtual void LoadGameModeInfo(GameModeInfo info)
    {
        levelInfo = info;
        playerInjuryMultiplier = info.playerInjuryMultiplier;
        enemyInjuryMultiplier = info.enemyInjuryMultiplier;
    }

}
