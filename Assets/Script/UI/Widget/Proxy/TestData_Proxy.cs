using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData_Proxy : Proxy
{
    public const string proxyName = "TestData1";
    public TestData testData;
    public TestData_Proxy() : base(proxyName)
    {
        testData = new TestData();
    }
    public void Add()
    {
        testData.Num++;
        SendNotification("msg_add", testData);
    }
    public void Sub()
    {
        testData.Num--;
        SendNotification("msg_sub", testData);
    }
}