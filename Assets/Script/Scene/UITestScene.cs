using MoleMole;
using SceneFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestScene : BaseScene
{
    public UITestScene() : base("UITestScene")
    {

    }

    public override void OnSceneLoaded()
    {
        UIManager.Instance.Push(new BaseContext(TestPanel.uiType));
    }
}
