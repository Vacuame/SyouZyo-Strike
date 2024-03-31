using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData_Command_Add : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        TestData_Proxy proxy = Facade.RetrieveProxy("TestData1") as TestData_Proxy;
        proxy.Add();
    }
}
public class TestData_Command_Sub : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        TestData_Proxy proxy = Facade.RetrieveProxy("TestData1") as TestData_Proxy;
        proxy.Sub();
    }
}
