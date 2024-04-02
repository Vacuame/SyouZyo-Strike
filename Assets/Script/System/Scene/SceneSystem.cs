using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneFramework
{
    /// <summary>
    /// 场景状态管理系统
    /// </summary>
    public class SceneSystem:Singleton<SceneSystem>
    {
        public BaseScene curScene;
        private AsyncSceneLoader loader;
        private readonly string loaderPrefabPath = "UI/AsyncSceneLoader";
        public void Init()
        {
            GameRoot.Instance.afterLoadSceneAction += () => { curScene?.OnSceneLoaded();};
        }

        public void SetScene(BaseScene scene)
        {
            curScene?.OnExit();
            GameRoot.Instance.beforeLoadSceneAction?.Invoke();
            curScene = scene;
            SceneManager.LoadScene(curScene.SceneName);
        }

        public void SetSceneAsync(BaseScene scene,float fadeTime = 1,bool needPressKey = true)
        {
            curScene?.OnExit();
            GameRoot.Instance.beforeLoadSceneAction?.Invoke();
            curScene = scene;
            if (loader == null)
            {
                loader = GameObject.Instantiate(Resources.Load<AsyncSceneLoader>(loaderPrefabPath));
            }

            loader.DoLoad(curScene.SceneName,fadeTime,needPressKey);
        }

    }
}
