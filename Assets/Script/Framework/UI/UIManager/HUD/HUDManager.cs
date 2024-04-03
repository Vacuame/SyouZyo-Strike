using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoleMole
{
    public class HUDManager:Singleton<HUDManager>
    {
        private const string HUDRootPath = "UI/HUD/";

        private Dictionary<string, BaseHUD> _HUDDict = new Dictionary<string, BaseHUD>();

        private Transform _canvas;
        public override void Init()
        {
            GameRoot.Instance.afterLoadSceneAction += () => { OnSceneLoaded(); };
        }
        public void OnSceneLoaded()
        {
            GameObject canvasObj = GameObject.Find("HUDCanvas");
            if (canvasObj != null)
            {
                _canvas = canvasObj.transform;
                for (int i = 0; i < _canvas.childCount; i++)
                {
                    GameObject.Destroy(_canvas.GetChild(i).gameObject);
                }
            }
        }

        public static T GetHUD<T>()where T : BaseHUD
        {
            if (GameRoot.ApplicationQuit)
                return null;

            string name = typeof(T).Name;
            Dictionary<string, BaseHUD> HUDDict = Instance._HUDDict;
            if (HUDDict.ContainsKey(name) == false || HUDDict[name] == null)
            {
                BaseHUD hud = GameObject.Instantiate(Resources.Load<BaseHUD>(HUDRootPath + name));
                hud.transform.SetParent(Instance._canvas, false);
                hud.name = name;
                HUDDict.AddOrReplace(name, hud);
                hud.OnEnter();
                return hud as T;
            }
            return HUDDict[name] as T;
        }
    }

}
