using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一直存在的游戏系统类，管理其他系统
/// </summary>
public class GameSystem : SingletonMono<GameSystem>
{
    protected override bool dontDestroyOnLoad => true;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Consts.ApplicationIsQuitting = true;
    }
}
