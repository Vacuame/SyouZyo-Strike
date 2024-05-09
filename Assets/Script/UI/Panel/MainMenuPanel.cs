using MyUI;
using MyScene;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("MainMenuPanel");

    public Button btnSt;
    public Button btnExit;

    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);
        btnSt.onClick.AddListener(GameStart);
        btnExit.onClick.AddListener(Exit);
    }

    private void GameStart()
    {
        SceneSystem.Instance.SetSceneAsync(new BaseScene("Level1"));
    }
    private void Exit()
    {
        Application.Quit();
    }

}
