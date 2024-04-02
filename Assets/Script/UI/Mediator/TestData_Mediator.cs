using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestData_Mediator : Mediator
{
    public const string mediatorName = "testMediator";
    TestPanel view;
    public TestData_Mediator(TestPanel panel) : base(mediatorName)
    {
        this.view = panel;
        view.btnAdd.onClick.AddListener(() => SendNotification("cmd_add"));
        view.btnSub.onClick.AddListener(() => SendNotification("cmd_sub"));
    }

    public override string[] ListNotificationInterests()
    {
        return new string[] {"msg_dataChange",};
    }

    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
            case "msg_dataChange":
                Display(notification.Body as TestData);
                break;
        }
    }

    public void Display(TestData data)
    {
        view.txtNum.text = data.Num.ToString();
    }

}
