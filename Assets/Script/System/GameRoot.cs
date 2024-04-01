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
    protected override bool dontDestroyOnLoad => true;

    public UnityAction beforeLoadSceneAction;

    /// <summary>
    /// ����ʼ��
    /// ע���ʼ��˳��Scene�����UI������UI�ȳ�ʼ������sceneLoaded��ִ�У�
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
