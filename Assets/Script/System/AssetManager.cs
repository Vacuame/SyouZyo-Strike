using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.ReloadAttribute;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class AssetManager : SingletonMono<AssetManager>
{
    private AssetBundle _mainAb; 
    private AssetBundle mainAb
    { get { if (_mainAb == null) _mainAb = AssetBundle.LoadFromFile(abRootPath + mainAbName); return _mainAb; } }

    private AssetBundleManifest _mainFest; 
    private AssetBundleManifest mainFest
    { get { if (_mainFest == null) _mainFest = mainAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");return _mainFest;  } }

    private string abRootPath = Application.streamingAssetsPath + "/";
    private string mainAbName = "PC";
    private Dictionary<string, AssetBundle> bundleDic=new Dictionary<string, AssetBundle>();

    #region Bundle异同步加载
    public bool LoadBundle(string bundleName)
    {
        if (bundleDic.ContainsKey(bundleName)) return true;

        foreach (var dependencyName in mainFest.GetAllDependencies(bundleName))
        {
            if (bundleDic.ContainsKey(dependencyName)) continue;
            var dependency = AssetBundle.LoadFromFile(abRootPath + dependencyName);
            bundleDic.Add(dependencyName, dependency);
        }

        var bundle = AssetBundle.LoadFromFile(abRootPath + bundleName);
        if (bundle == null) return false;
        bundleDic.Add(bundleName, bundle);
        return true;
    }
    public Coroutine LoadBundleAsync(string bundleName)
    {
        return StartCoroutine(LoadBundleAsyncCoroutine(bundleName));
    }
    private IEnumerator LoadBundleAsyncCoroutine(string bundleName)
    {
        if (bundleDic.ContainsKey(bundleName)) yield break;

        foreach (var dependencyName in mainFest.GetAllDependencies(bundleName))
        {
            if (bundleDic.ContainsKey(dependencyName)) continue;
            var dependencyLoad = AssetBundle.LoadFromFileAsync(abRootPath + dependencyName);
            yield return dependencyLoad;
            bundleDic.Add(dependencyName, dependencyLoad.assetBundle);
        }

        var resLoad = AssetBundle.LoadFromFileAsync(abRootPath + bundleName);
        yield return resLoad;
        bundleDic.Add(bundleName, resLoad.assetBundle);
        yield return null;
    }
    #endregion

    #region Asset同步加载
    public Object LoadAsset(string bundleName,string assetName)
    {
        LoadBundle(bundleName);
        return bundleDic[bundleName].LoadAsset(assetName);
    }
    public Object LoadAsset(string bundleName, string assetName,System.Type type)
    {
        LoadBundle(bundleName);
        return bundleDic[bundleName].LoadAsset(assetName,type);
    }
    public T LoadAsset<T>(string bundleName, string assetName)where T:Object
    {
        LoadBundle(bundleName);
        return bundleDic[bundleName].LoadAsset<T>(assetName);
    }
    #endregion

    #region Asset异步加载
    public void LoadAssetAsync(string bundleName, string assetName,UnityAction<object> onLoadedEvent)
    {
        StartCoroutine(LoadAssetAsyncCoroutine(bundleName,assetName, onLoadedEvent));
    }
    private IEnumerator LoadAssetAsyncCoroutine(string bundleName, string assetName, UnityAction<object> onLoadedEvent)
    {
        yield return LoadBundleAsync(bundleName);
        var assetLoad = bundleDic[bundleName].LoadAssetAsync(assetName);
        yield return assetLoad;
        onLoadedEvent?.Invoke(assetLoad.asset);
        yield return null;
    }

    public void LoadAssetAsync(string bundleName, string assetName, UnityAction<object> onLoadedEvent,System.Type type)
    {
        StartCoroutine(LoadAssetAsyncCoroutine(bundleName, assetName, onLoadedEvent,type));
    }
    private IEnumerator LoadAssetAsyncCoroutine(string bundleName, string assetName, UnityAction<object> onLoadedEvent, System.Type type)
    {
        yield return LoadBundleAsync(bundleName);
        var assetLoad = bundleDic[bundleName].LoadAssetAsync(assetName,type);
        yield return assetLoad;
        onLoadedEvent?.Invoke(assetLoad.asset);
        yield return null;
    }

    public void LoadAssetAsync<T>(string bundleName, string assetName, UnityAction<T> onLoadedEvent) where T:Object
    {
        StartCoroutine(LoadAssetAsyncCoroutine(bundleName, assetName, onLoadedEvent));
    }
    private IEnumerator LoadAssetAsyncCoroutine<T>(string bundleName, string assetName, UnityAction<T> onLoadedEvent)where T:Object
    {
        yield return LoadBundleAsync(bundleName);
        var assetLoad = bundleDic[bundleName].LoadAssetAsync<T>(assetName);
        yield return assetLoad;
        onLoadedEvent?.Invoke(assetLoad.asset as T);
        yield return null;
    }

    #endregion
}
