using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.facade.RegisterGroupCommand(new TestData_Command());
        UIManager.Instance.facade.RegisterMediator(new TestData_Mediator(gameObject));
        UIManager.Instance.facade.RegisterProxy(new TestData_Proxy());
    }

    private void OnDestroy()
    {
        UIManager.Instance.facade.RemoveGroupCommand(TestData_Command.commandNames);
        UIManager.Instance.facade.RemoveMediator(TestData_Mediator.mediatorName);
        UIManager.Instance.facade.RemoveProxy(TestData_Proxy.proxyName);
    }

}
