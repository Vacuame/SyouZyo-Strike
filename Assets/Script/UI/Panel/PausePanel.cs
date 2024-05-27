using MyScene;
using MyUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    public readonly static UIType uiType = new UIType("PausePanel");

    [SerializeField] private Button btnExit;

    bool toMainScene = false;

    protected override void Init()
    {
        btnExit.onClick.AddListener(Exit);
    }

    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void OnExit(bool trueDestroy)
    {
        if(!toMainScene)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        base.OnExit(trueDestroy);
    }

    private void Exit()
    {
        toMainScene = true;
        SceneSystem.Instance.SetSceneAsync(new MainMenuScene(), needPressKey: false);
    }
}
