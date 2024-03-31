using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : MonoBehaviour
{
    TestFacade facade;
    private void Start()
    {
        facade = new TestFacade(gameObject);
    }
}
