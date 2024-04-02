using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 注意如果不是dontDestroyOnLoad，则每个场景都会执行Init
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
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
        {
            instance = (T)(object)this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        Init();
    }

    protected virtual void Init()
    {

    }

    public static T GetOrCreateInstance()
    {
        if (instance == null && Application.isPlaying)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = typeof(T).Name;
            gameObject.AddComponent<T>();
        }
        return instance;
    }
}
