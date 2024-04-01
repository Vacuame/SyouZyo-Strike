using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using PureMVC.Patterns.Observer;
using SceneFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.Mesh;

public class TestData_Command : GroupCommand
{
    public static string[] commandNames = { "cmd_add", "cmd_sub" };
    public override SubCommandRegister[] CommandList()
    {
        return new SubCommandRegister[] {
        new SubCommandRegister("cmd_add",Add),
        new SubCommandRegister("cmd_sub",Sub)
        };
    }
    public void Add(INotification notification)
    {
        TestData_Proxy proxy = Facade.RetrieveProxy("TestData1") as TestData_Proxy;
        proxy.Add();
        if(proxy.testData.Num==3)
        {
            SceneSystem.Instance.SetSceneAsync(new UITestScene2());
        }
    }
    public void Sub(INotification notification)
    {
        TestData_Proxy proxy = Facade.RetrieveProxy("TestData1") as TestData_Proxy;
        proxy.Sub();
    }
}
