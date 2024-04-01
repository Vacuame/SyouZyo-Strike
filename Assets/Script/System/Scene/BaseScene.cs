using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoleMole;

namespace SceneFramework
{
    /// <summary>
    /// 场景状态
    /// </summary>
    public class BaseScene
    {
        protected string sceneName = "";
        public string SceneName { get => sceneName; }

        public BaseScene(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public virtual void OnSceneLoaded()
        {

        }

        public virtual void OnExit()
        {

        }
    }
}
