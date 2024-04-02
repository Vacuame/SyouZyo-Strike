using System.Collections.Generic;
using UnityEngine;
namespace MoleMole
{
    public class HUDManager:Singleton<HUDManager>
    {
        private const string HUDRootPath = "UI/HUD";

        private Dictionary<UIType, BaseHUD> HUDDict = new Dictionary<UIType, BaseHUD>();

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

        public BaseHUD GetHUD(BaseContext context) 
        {
            UIType uiType = context.uiType;
            if (HUDDict.ContainsKey(uiType) == false || HUDDict[uiType] == null)
            {
                BaseHUD hud = GameObject.Instantiate(Resources.Load<BaseHUD>(HUDRootPath + uiType.Path));
                hud.transform.SetParent(_canvas, false);
                hud.name = uiType.Name;
                HUDDict.AddOrReplace(uiType, hud);
                hud.OnInstance(context);
                return hud;
            }
            return HUDDict[uiType];
        }
    }

}
