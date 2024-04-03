using MoleMole;
using SceneFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// һֱ���ڵ���Ϸϵͳ�࣬��������ϵͳ
/// </summary>
public class GameRoot : SingletonMono<GameRoot>
{
    public static bool ApplicationQuit => !avaiable;
    protected override bool dontDestroyOnLoad => true;

    /// <summary>
    /// ������Ҫ�ڼ��س���ǰ����õĺ����ŵ���Action��Ȼ���ڼ��س���ǰ�������
    /// </summary>
    public UnityAction beforeLoadSceneAction;
    public UnityAction afterLoadSceneAction;

    /// <summary>
    /// ����ʼ��
    /// ע���ʼ��˳��Scene�����UI������UI�ȳ�ʼ������sceneLoaded��ִ�У�
    /// </summary>
    protected override void Init()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => afterLoadSceneAction?.Invoke();

        UIManager.Instance.Init();
        HUDManager.Instance.Init();
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
}
