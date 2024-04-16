using System;
using System.Collections;
using UnityEngine;


[Serializable]
public struct Pair<T, S>
{
    public T key; public S value;

    public Pair(T k, S v)
    {
        key = k;
        value = v;
    }
}
