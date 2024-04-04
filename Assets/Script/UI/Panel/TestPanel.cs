using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("TestView");
    public static readonly UIType uiType2 = new UIType("TestView2");
    public Text txtNum;
    public Button btnAdd, btnSub;
    protected override void Init()
    {
        txtNum = transform.Find("txtNum").GetComponent<Text>();
        btnAdd = transform.Find("btnAdd").GetComponent<Button>();
        btnSub = transform.Find("btnSub").GetComponent<Button>();
    }

    public override void OnEnter(PanelContext context)
    {
        base.OnEnter(context);
        UIManager.facade.RegisterGroupCommand(new TestData_Command());
        UIManager.facade.RegisterMediator(new TestData_Mediator(this));
        UIManager.facade.RegisterProxy(new TestData_Proxy());
    }
    public override void OnExit(bool trueDestroy)
    {
        base.OnExit(trueDestroy);
        UIManager.facade.RemoveGroupCommand(TestData_Command.commandNames);
        UIManager.facade.RemoveMediator(TestData_Mediator.mediatorName);
        UIManager.facade.RemoveProxy(TestData_Proxy.proxyName);
    }
}
