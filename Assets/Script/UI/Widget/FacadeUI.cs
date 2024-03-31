using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacadeUI : SingletonMono<FacadeUI>
{
    public TestFacade facade { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        facade = new TestFacade();
    }
}
