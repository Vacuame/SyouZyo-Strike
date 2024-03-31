using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : MonoBehaviour
{
    void Start()
    {
        FacadeUI.Instance.facade.RegisterGroupCommand(new TestData_Command());
        FacadeUI.Instance.facade.RegisterMediator(new TestData_Mediator(gameObject));
        FacadeUI.Instance.facade.RegisterProxy(new TestData_Proxy());
    }

    private void OnDestroy()
    {
        FacadeUI.Instance.facade.RemoveGroupCommand(TestData_Command.commandNames);
        FacadeUI.Instance.facade.RemoveMediator(TestData_Mediator.mediatorName);
        FacadeUI.Instance.facade.RemoveProxy(TestData_Proxy.proxyName);
    }

}
