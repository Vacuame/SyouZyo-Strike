using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Play : GameMode
{
    protected List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector] public PlayerController playerController;
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

}
