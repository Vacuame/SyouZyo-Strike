using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public Transform follows;
    protected void Update()
    {
        nav.destination = follows.position;
        nav.stoppingDistance = 1;
    }
}
