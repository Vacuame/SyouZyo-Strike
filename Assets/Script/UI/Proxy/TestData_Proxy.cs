using PureMVC.Interfaces;
using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData_Proxy : Proxy
{
    public const string proxyName = "TestData1";
    public TestData testData { get; private set; }
    public TestData_Proxy() : base(proxyName)
    {
        testData = new TestData();
    }
    public void Add()
    {
        testData.Num++;
        SendNotification("msg_dataChange", testData);
    }
    public void Sub()
    {
        testData.Num--;
        SendNotification("msg_dataChange", testData);
    }
}