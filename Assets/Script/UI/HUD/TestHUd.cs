using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHUd : WorldHUD
{
    public void Move(Vector3 pos)
    {
        transform.position = pos;
    }
}
