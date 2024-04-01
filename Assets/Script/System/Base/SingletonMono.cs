using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 注意如果不是dontDestroyOnLoad，则每个场景都会执行Init
/// </summary>
public class SingletonMono<T>: MonoBehaviour where T : MonoBehaviour
{
    protected virtual bool dontDestroyOnLoad => false;
    protected static T instance;
    public static T Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = (T)(object)this;
        else
            Destroy(gameObject);

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        if(this == instance)
            Init();

    }

    protected virtual void Init()
    {

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