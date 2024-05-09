using MyUI;
using MyScene;
using UnityEngine;

public class MainMenuScene : BaseScene
{
    public MainMenuScene() : base("MainMenu")
    {

    }

    public override void OnSceneLoaded()
    {
        UIManager.Instance.Push(new PanelContext(MainMenuPanel.uiType));
    }
}
