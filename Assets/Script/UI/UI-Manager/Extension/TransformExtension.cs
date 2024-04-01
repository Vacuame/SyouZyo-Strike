﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoleMole
{
	public static class TransformExtension 
    {
        public static void DestroyChildren(this Transform trans)
        {
            foreach (Transform child in trans)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        public static Transform AddChildFromPrefab(this Transform trans, Transform prefab, string name = null)
        {
            Transform childTrans = GameObject.Instantiate(prefab) as Transform;
            childTrans.SetParent(trans, false);
            if (name != null)
	        {
                childTrans.gameObject.name = name;
	        }
            return childTrans;
        }
        public static T GetOrAddComponent<T>(this Transform t) where T : Component
        {
            T component = t.GetComponent<T>();
            if (component == null)
                component = t.gameObject.AddComponent<T>();

            return component;
        }
        public static void PanelAppearance(this Transform t, bool on_off, bool active = false)
        {
            CanvasGroup group = t.GetOrAddComponent<CanvasGroup>();
            int value = on_off == true ? 1 : 0;

            //射线检测
            group.blocksRaycasts = on_off;
            //交互
            group.interactable = on_off;
            //透明度
            group.alpha = value;

            t.gameObject.SetActive(on_off || active);
        }
    }
}
