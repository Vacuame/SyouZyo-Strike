using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestView : BaseView
{
    public static readonly UIType uiType = new UIType("TestView");
    public static readonly UIType uiType2 = new UIType("TestView2");

    public override void OnEnter(BaseContext context)
    {
        UIManager.Instance.facade.RegisterGroupCommand(new TestData_Command());
        UIManager.Instance.facade.RegisterMediator(new TestData_Mediator(gameObject));
        UIManager.Instance.facade.RegisterProxy(new TestData_Proxy());
    }
    public override void OnExit(BaseContext context)
    {
        UIManager.Instance.facade.RemoveGroupCommand(TestData_Command.commandNames);
        UIManager.Instance.facade.RemoveMediator(TestData_Mediator.mediatorName);
        UIManager.Instance.facade.RemoveProxy(TestData_Proxy.proxyName);
    }
}
