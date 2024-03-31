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
    public Text txtNum;
    public Button btnAdd, btnSub;
    public TestData_Mediator(GameObject panel) : base(mediatorName)
    {
        txtNum = panel.transform.Find("txtNum").GetComponent<Text>();
        btnAdd = panel.transform.Find("btnAdd").GetComponent<Button>();
        btnSub = panel.transform.Find("btnSub").GetComponent<Button>();
        btnAdd.onClick.AddListener(() => SendNotification("cmd_add"));
        btnSub.onClick.AddListener(() => SendNotification("cmd_sub"));
    }

    public override string[] ListNotificationInterests()
    {
        return new string[] { 
            "msg_add",
            "msg_sub" };
    }

    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name)
        {
            case "msg_add":
                Display(notification.Body as TestData);
                break;
            case "msg_sub":
                Display(notification.Body as TestData);
                break;
        }
    }

    public void Display(TestData data)
    {
        txtNum.text = data.Num.ToString();
    }

}
