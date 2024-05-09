using MyUI;
using MyScene;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// һֱ���ڵ���Ϸϵͳ�࣬��������ϵͳ
/// </summary>
public class GameRoot : SingletonMono<GameRoot>
{
    public bool testGame;
    public static bool ApplicationQuit => !avaiable;
    protected override bool dontDestroyOnLoad => true;

    /// <summary>
    /// ������Ҫ�ڼ��س���ǰ����õĺ����ŵ���Action��Ȼ���ڼ��س���ǰ�������
    /// </summary>
    public UnityAction beforeLoadSceneAction;
    public UnityAction afterLoadSceneAction;

    public GameMode gameMode;

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

        if(testGame)
        {
            SceneSystem.Instance.curScene = new LevelScene("Level1");
        }
        else
        {
            SceneSystem.Instance.curScene= new MainMenuScene();
        }
    }

    void Update()
    {
        TimerManager.Instance.Update();
    }
}
