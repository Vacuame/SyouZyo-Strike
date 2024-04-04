using System.Collections;
using UnityEngine;


public class Singleton<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
    /// <summary>
    /// 需要被调用的才Init，懒加载的用构造函数
    /// </summary>
    public virtual void Init()
    {

    }
}