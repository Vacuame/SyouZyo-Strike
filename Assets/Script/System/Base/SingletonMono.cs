﻿using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonMono<T>: MonoBehaviour where T : MonoBehaviour
{
    protected virtual bool dontDestroyOnLoad => false;
    protected static T instance;
    public static T Instance
    {
        get => instance;
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = (T)(object)this;
        else
            Destroy(gameObject);

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

    }

}

public class SingletonMono_AutoInst<T>:SingletonMono<T> where T:MonoBehaviour
{
    new public static T Instance
    {
        get
        {
            if (instance == null)
            {
                if (Consts.ApplicationIsQuitting)
                    return null;
                GameObject newObj = new GameObject();
                instance = newObj.AddComponent<T>();
            }
            return instance;
        }
    }
}