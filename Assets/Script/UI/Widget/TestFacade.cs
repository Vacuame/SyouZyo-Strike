using PureMVC.Patterns.Facade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFacade : Facade
{
    public TestFacade(GameObject pannel):base() 
    {
        RegisterCommand("cmd_add", () => { return new TestData_Command_Add(); });
        RegisterCommand("cmd_sub", () => { return new TestData_Command_Sub(); });

        RegisterMediator(new TestData_Mediator(pannel));
        RegisterProxy(new TestData_Proxy());
    }
}
