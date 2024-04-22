using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTEST : MonoBehaviour
{
    CharacterController cc;
    public float g = 9.8f;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        cc.Move(g * Time.fixedDeltaTime * Vector3.down);
    }
}
