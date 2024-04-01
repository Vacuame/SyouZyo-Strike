using MoleMole;
using SceneFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 一直存在的游戏系统类，管理其他系统
/// </summary>
public class GameRoot : SingletonMono<GameRoot>
{
    protected override bool dontDestroyOnLoad => true;

    public UnityAction beforeLoadSceneAction;

    /// <summary>
    /// 许多初始化
    /// 注意初始化顺序，Scene会调用UI，所以UI先初始化（绑定sceneLoaded先执行）
    /// </summary>
    protected override void Init()
    {
        UIManager.Instance.Init();
        SceneSystem.Instance.Init();

#if UNITY_EDITOR
        SceneSystem.Instance.curScene = new UITestScene();
#else
        //SceneSystem.Instance.SetScene("MainScene")
#endif
    }

    void Update()
    {
        TimerManager.Instance.Update();
    }

    private void OnDestroy()
    {
        Consts.ApplicationIsQuitting = true;
    }
}
