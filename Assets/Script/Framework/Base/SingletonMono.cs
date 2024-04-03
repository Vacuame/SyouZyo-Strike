using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 注意如果不是dontDestroyOnLoad，则每个场景都会执行Init
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    protected virtual bool dontDestroyOnLoad => false;
    protected static T instance;
    protected static bool avaiable = true;
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

        //sceneUnloaded说明GameObject被清空了，不会有人调用了，所以设置avaiable
        //若在sceneLoaded设置就晚了，因为在Awake之后执行，Awake调用就拿不到Instance
        SceneManager.sceneUnloaded += (Scene) => { avaiable = true; };

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        Init();
    }

    protected virtual void Init()
    {

    }

    public static T GetOrCreateInstance()
    {
        if (!avaiable) 
            return null;

        if (instance == null && Application.isPlaying)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = typeof(T).Name;
            gameObject.AddComponent<T>();
        }
        return instance;
    }

    private void OnDestroy()
    {
        avaiable = false;
    }
    private void OnApplicationQuit()
    {
        avaiable = false;
    }

}
