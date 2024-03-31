using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using PureMVC.Patterns.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Mesh;

public class TestData_Command : GroupCommand
{
    public static string[] commandNames = { "s" };
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
    }
    public void Sub(INotification notification)
    {
        TestData_Proxy proxy = Facade.RetrieveProxy("TestData1") as TestData_Proxy;
        proxy.Sub();
    }
}

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
