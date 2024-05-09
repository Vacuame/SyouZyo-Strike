using MyUI;
using MyScene;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 一直存在的游戏系统类，管理其他系统
/// </summary>
public class GameRoot : SingletonMono<GameRoot>
{
    public bool testGame;
    public static bool ApplicationQuit => !avaiable;
    protected override bool dontDestroyOnLoad => true;

    /// <summary>
    /// 所有需要在加载场景前后调用的函数放到此Action，然后在加载场景前后调用它
    /// </summary>
    public UnityAction beforeLoadSceneAction;
    public UnityAction afterLoadSceneAction;

    public GameMode gameMode;

    /// <summary>
    /// 许多初始化
    /// 注意初始化顺序，Scene会调用UI，所以UI先初始化（绑定sceneLoaded先执行）
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
