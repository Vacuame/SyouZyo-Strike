using MyUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissonHUD_SurviveInWave : BaseHUD
{
    [SerializeField] private Text timerText;
    [SerializeField] private Text enemyKillText;

    public void SetTimerText(float time)
    {
        int minute = (int)(time / 60);
        float seconed = time % 60;
        timerText.text = $"{minute}:{seconed.ToString("0.00")}";
    }

    public void SetEnemyKillText(int killed,int sum)
    {
        enemyKillText.text = $"{killed} / {sum}";
    }

}
