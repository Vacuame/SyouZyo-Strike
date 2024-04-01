using MoleMole;
using SceneFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestScene2 : BaseScene
{
    public UITestScene2() : base("UITestScene2")
    {
        
    }
    public override void OnSceneLoaded()
    {
        base.OnSceneLoaded();

        UIManager.Instance.Push(new BaseContext(TestView.uiType2));
    }

}
