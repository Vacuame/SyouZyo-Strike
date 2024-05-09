using MyUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


//发现玩家会通知所有敌人
//而且会刷怪
//还有计时
public class GameMode_Play_Level1 : GameMode_Play
{
    private bool bBattle,bWin;
    [SerializeField] private float timeToWin;
    private float winTimer;

    private int killedEnemyNum;
    [SerializeField] private int sumEnemyNum;
    [SerializeField] private int maxEnemyNumInScene;

    private bool trigerSpawnEnemy;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private Enemy enemyPrefab;

    [HideInInspector]public GameObject enemiesTarget;

    private void Update()
    {
        if(bBattle)
        {
            winTimer.TimerTick();

            //更新Timer HUD
            MissonHUD_SurviveInWave hud = HUDManager.GetHUD<MissonHUD_SurviveInWave>();
            hud.SetTimerText(winTimer);
            hud.SetEnemyKillText(killedEnemyNum, sumEnemyNum);

            if(winTimer<=0)//胜利
            {
                Win();
            }
        }

        if (trigerSpawnEnemy)
        {
            trigerSpawnEnemy = false;
            int spawnNum = Mathf.Min(maxEnemyNumInScene - enemyList.Count, sumEnemyNum - enemyList.Count - killedEnemyNum);
            while (spawnNum > 0)
            {
                SpawnEnemy();
                spawnNum--;
            }
        }
    }

    public override void OnNotifyPlayer(GameObject playerObj)
    {
        enemiesTarget = playerObj;
        foreach (var e in enemyList)
        {
            EventManager.Instance.TriggerEvent("Hear" + e.gameObject.GetInstanceID(), enemiesTarget.transform.position, 
                new SoundInfo(SoundType.NotifyPlayer, enemiesTarget));
        }

        //初始化变量
        bBattle = true;
        trigerSpawnEnemy = true;
        winTimer = timeToWin;

        //BGM

    }

    public override void OnEnemySpawn(Enemy e)
    {
        base.OnEnemySpawn(e);

        if (enemiesTarget != null)
        {
            EventManager.Instance.TriggerEvent("Hear" + e.gameObject.GetInstanceID(),
                enemiesTarget.transform.position,
                new SoundInfo(SoundType.NotifyPlayer, enemiesTarget));
        }
    }

    public override void OnEnemyDead(Enemy e)
    {
        base.OnEnemyDead(e);

        if (bWin) return;

        killedEnemyNum++;
        
        if(bBattle)
        {
            trigerSpawnEnemy = true;
        }

        if(killedEnemyNum == sumEnemyNum)//胜利
        {
            Win();
        }

    }

    private void Win()
    {
        bWin = true;
        bBattle = false;

        for(int i = enemyList.Count-1;i>=0;i--)
        {
            Enemy enemy = enemyList[i];
            enemy.Dead();
        }

        UIManager.Instance.Push(new PanelContext(GameOverPanel.uiType));

        GameRoot.Instance.GetGameMode<GameMode_Play>().playerController.ReleasePawn();
    }

    private void SpawnEnemy()
    {
        int spawnTransIndex = Random.Range(0, enemySpawnPoints.Count);
        Transform spawnTrans = enemySpawnPoints[spawnTransIndex];
        Enemy newEnemy = GameObject.Instantiate(enemyPrefab, spawnTrans.position, spawnTrans.rotation);
    }

}
