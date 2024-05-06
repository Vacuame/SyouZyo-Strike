using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoleMole
{
    public class HUDManager : Singleton<HUDManager>
    {
        private const string HUDRootPath = "UI/HUD/";

        private Dictionary<string, BaseHUD> _HUDDict = new Dictionary<string, BaseHUD>();

        public Transform _canvas;
        public Transform _worldCanvas;

        public override void Init()
        {
            GameRoot.Instance.afterLoadSceneAction += () => { OnSceneLoaded(); };
        }
        public void OnSceneLoaded()
        {
            _canvas = GameObject.Find("HUDCanvas")?.transform;
            _worldCanvas = GameObject.Find("WorldCanvas")?.transform;
            Transform[] canvases = new Transform[2] { _canvas, _worldCanvas };
            foreach (var c in canvases)
            {
                for (int i = 0; i < c.childCount; i++)
                {
                    GameObject.Destroy(c.GetChild(i).gameObject);
                }
            }
        }

        public static T GetHUD<T>(bool inWorldHUD = false) where T : BaseHUD
        {
            if (GameRoot.ApplicationQuit)
                return null;

            string name = typeof(T).Name;
            Dictionary<string, BaseHUD> HUDDict = Instance._HUDDict;
            if (HUDDict.ContainsKey(name) == false || HUDDict[name] == null)
            {
                BaseHUD hud = GameObject.Instantiate(Resources.Load<BaseHUD>(HUDRootPath + name));
                Transform canvas = inWorldHUD ? Instance._worldCanvas : Instance._canvas;
                hud.transform.SetParent(canvas, false);
                hud.name = name;
                HUDDict.AddOrReplace(name, hud);
                hud.OnEnter();
                return hud as T;
            }
            return HUDDict[name] as T;
        }
    }

}
