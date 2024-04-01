using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;

namespace MoleMole
{
    public class UIManager :Singleton<UIManager>
    {
        private const string UIRootPath = "UI/";

        private Stack<BaseContext> _contextStack = new Stack<BaseContext>();

        public Dictionary<UIType, BaseView> _UIDict = new Dictionary<UIType, BaseView>();

        private Transform _canvas;

        public TestFacade facade { get; private set; }
        public void Init()
        {
            SceneManager.sceneUnloaded += (Scene scene) => { PopAll(); };
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { OnSceneLoaded(); };
            facade = new TestFacade();
        }

        public void OnSceneLoaded()
        {
            GameObject canvasObj = GameObject.Find("Canvas");
            if(canvasObj != null )
            {
                _canvas = canvasObj.transform;
                for(int i=0;i<_canvas.childCount;i++) 
                {
                    GameObject.Destroy(_canvas.GetChild(i).gameObject);
                }
            }
            
        }
        private BaseView GetView(UIType uiType)
        {
            if (_UIDict.ContainsKey(uiType) == false || _UIDict[uiType] == null)
            {
                BaseView go = GameObject.Instantiate(Resources.Load<BaseView>(UIRootPath + uiType.Path));
                go.transform.SetParent(_canvas, false);
                go.name = uiType.Name;
                _UIDict.AddOrReplace(uiType, go);
                return go;
            }
            return _UIDict[uiType];
        }

        public void DestroyView(UIType uiType,bool trueDestroy = false)
        {
            if (!_UIDict.ContainsKey(uiType))
                return;

            if (_UIDict[uiType] == null)
            {
                _UIDict.Remove(uiType);
                return;
            }

            if (trueDestroy)
            {
                GameObject.Destroy(_UIDict[uiType]);
                _UIDict.Remove(uiType);
            }
            else
            {
                _UIDict[uiType].transform.PanelAppearance(false);
            }
        }

        public void Push(BaseContext nextContext)
        {
            if (_contextStack.Count > 0)
            {
                BaseContext curContext = _contextStack.Peek();
                BaseView curView = GetView(curContext.ViewType);
                curView.OnPause(curContext);
            }

            _contextStack.Push(nextContext);
            BaseView nextView = GetView(nextContext.ViewType);
            nextView.OnEnter(nextContext);
        }

        public void Pop()
        {
            if (_contextStack.Count > 0)
            {
                BaseContext curContext = _contextStack.Peek();
                _contextStack.Pop();

                BaseView curView = GetView(curContext.ViewType);
                curView.OnExit(curContext);
            }

            if (_contextStack.Count > 0)
            {
                BaseContext lastContext = _contextStack.Peek();
                BaseView curView = GetView(lastContext.ViewType);
                curView.OnResume(lastContext);
            }
        }

        public void PopAll()
        {
            while( _contextStack.Count > 0)
                _contextStack.Pop();
        }
    }
}
