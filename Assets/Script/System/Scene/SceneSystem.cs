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
        private readonly string loaderPrefabPath = "";
        public void Init()
        {
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { curScene?.OnSceneLoaded();};
        }

        public void SetScene(BaseScene scene, bool async = true)
        {
            curScene?.OnExit();
            curScene = scene;
            if (async)
                LoadSceneAsync();
            else
                LoadScene();
        }

        protected void LoadScene()
        {
            SceneManager.LoadScene(curScene.SceneName);
        }

        protected void LoadSceneAsync()
        {
            if (loader == null)
            {
                loader = GameObject.Instantiate(Resources.Load<AsyncSceneLoader>(loaderPrefabPath));
            }

            loader.enabled = true;
            loader.DoLoad(curScene.SceneName);
        }
    }
}
