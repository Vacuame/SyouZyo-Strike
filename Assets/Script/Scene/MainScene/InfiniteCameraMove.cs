using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteCameraMove : MonoBehaviour
{
    public Transform moveTrans;
    public Vector3 stPos, edPos;
    [HideInInspector]public float timer;
    public float circleTime;

    private void Update()
    {
        moveTrans.position = Vector3.Lerp(stPos, edPos, timer / circleTime);

        timer += Time.deltaTime;
        if (timer > circleTime)
            timer = 0;
    }
}
