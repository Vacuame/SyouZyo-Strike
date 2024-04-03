using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;

namespace MoleMole
{
    public class UIManager :Singleton<UIManager>
    {
        private const string UIRootPath = "UI/Panel/";

        private Stack<BaseContext> _contextStack = new Stack<BaseContext>();

        public Dictionary<UIType, BasePanel> _UIDict = new Dictionary<UIType, BasePanel>();

        private Transform _canvas;
        /// <summary>
        /// 如果用MVC，则需要Facade
        /// </summary>
        private UIFacade _facade;
        public static UIFacade facade => Instance._facade;
        public override void Init()
        {
            GameRoot.Instance.beforeLoadSceneAction += () => { PopAll(true); };
            GameRoot.Instance.afterLoadSceneAction += () => { OnSceneLoaded(); };
            _facade = new UIFacade();
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
        private BasePanel GetOrCreateView(UIType uiType)
        {
            if (_UIDict.ContainsKey(uiType) == false || _UIDict[uiType] == null)
            {
                BasePanel go = GameObject.Instantiate(Resources.Load<BasePanel>(UIRootPath + uiType.Path));
                go.transform.SetParent(_canvas, false);
                go.name = uiType.Name;
                _UIDict.AddOrReplace(uiType, go);
                return go;
            }
            return _UIDict[uiType];
        }

        public void DestroyView(UIType uiType)
        {
            if (!_UIDict.ContainsKey(uiType))
                return;

            if (_UIDict[uiType] != null)
            {
                GameObject.Destroy(_UIDict[uiType]);
            }

            _UIDict.Remove(uiType);
        }

        public void Push(BaseContext nextContext)
        {
            if (_contextStack.Count > 0)
            {
                BaseContext curContext = _contextStack.Peek();
                BasePanel curView = GetOrCreateView(curContext.uiType);
                curView.OnPause();
            }

            _contextStack.Push(nextContext);
            BasePanel nextView = GetOrCreateView(nextContext.uiType);
            nextView.OnEnter(nextContext);
        }

        public void Pop(bool trueDestroy = false)
        {
            if (_contextStack.Count > 0)
            {
                BaseContext curContext = _contextStack.Peek();
                _contextStack.Pop();

                if(_UIDict.TryGetValue(curContext.uiType,out BasePanel curView)&&curView!=null)
                    curView.OnExit(trueDestroy);
            }

            if (_contextStack.Count > 0)
            {
                BaseContext lastContext = _contextStack.Peek();
                BasePanel curView = GetOrCreateView(lastContext.uiType);
                curView.OnResume();
            }
        }

        public void SwitchOnPeek(BaseContext context)
        {
            if(_contextStack.TryPeek(out BaseContext cont) && cont.uiType == context.uiType)
                Pop(false);
            else
                Push(context);
        }

        public void PopAll(bool trueDestroy)
        {
            while (_contextStack.Count > 0)
                Pop(trueDestroy);
        }
    }
}
