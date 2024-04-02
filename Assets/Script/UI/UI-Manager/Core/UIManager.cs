﻿using UnityEngine;
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
            GameRoot.Instance.beforeLoadSceneAction += () => { PopAll(true); };
            GameRoot.Instance.afterLoadSceneAction += () => { OnSceneLoaded(); };
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
        private BaseView GetOrCreateView(UIType uiType)
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
                BaseView curView = GetOrCreateView(curContext.ViewType);
                curView.OnPause();
            }

            _contextStack.Push(nextContext);
            BaseView nextView = GetOrCreateView(nextContext.ViewType);
            nextView.OnEnter(nextContext);
        }

        public void Pop(bool trueDestroy = false)
        {
            if (_contextStack.Count > 0)
            {
                BaseContext curContext = _contextStack.Peek();
                _contextStack.Pop();

                if(_UIDict.TryGetValue(curContext.ViewType,out BaseView curView)&&curView!=null)
                    curView.OnExit(trueDestroy);
            }

            if (_contextStack.Count > 0)
            {
                BaseContext lastContext = _contextStack.Peek();
                BaseView curView = GetOrCreateView(lastContext.ViewType);
                curView.OnResume();
            }
        }

        public void PopAll(bool trueDestroy)
        {
            while (_contextStack.Count > 0)
                Pop(trueDestroy);
        }
    }
}
