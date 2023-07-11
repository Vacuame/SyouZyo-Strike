using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public void LoadBundle(string bundleName)
    {
        if (bundleDic.ContainsKey(bundleName)) return;

        foreach (var dependencyName in mainFest.GetAllDependencies(bundleName))
        {
            if (bundleDic.ContainsKey(dependencyName)) continue;
            var dependency = AssetBundle.LoadFromFile(abRootPath+dependencyName);
            bundleDic.Add(dependencyName, dependency);
        }

        var bundle = AssetBundle.LoadFromFile(abRootPath +bundleName);
        bundleDic.Add(bundleName, bundle);
    }

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



}
