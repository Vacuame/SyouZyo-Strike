using MoleMole;
using SceneFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// һֱ���ڵ���Ϸϵͳ�࣬��������ϵͳ
/// </summary>
public class GameRoot : SingletonMono<GameRoot>
{
    protected override bool dontDestroyOnLoad => true;
    private static bool GameInitialized = false;

    /// <summary>
    /// ����ʼ��
    /// ע���ʼ��˳��Scene�����UI������UI�ȳ�ʼ������sceneLoaded��
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        if (GameInitialized) return;
        UIManager.Instance.Init();
        SceneSystem.Instance.Init();
        GameInitialized = true;

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
