using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// һֱ���ڵ���Ϸϵͳ�࣬��������ϵͳ
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
