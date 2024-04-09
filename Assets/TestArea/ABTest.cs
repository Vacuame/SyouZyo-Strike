using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABTest : MonoBehaviour
{
    void Start()
    {
        
    }

    public void LoadWithDependency(string packName, string objName, Vector3 pos)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + $"/{packName}");
        AssetBundle mainAb = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/PC");

        var mainFest = mainAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        foreach(var a in mainFest.GetAllDependencies(packName))
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + $"/{a}");

        var prefab = ab.LoadAsset(objName, typeof(GameObject)) as GameObject;
        GameObject.Instantiate(prefab, pos, Quaternion.identity);
    }

    IEnumerator AbAsync(string packName,string objName,Vector3 pos)
    {
        var packLoad = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath+$"/{packName}");
        yield return packLoad;
        var prefabLoad = packLoad.assetBundle.LoadAssetAsync(objName,typeof(GameObject));
        yield return prefabLoad;
        GameObject prefab = prefabLoad.asset as GameObject;
        GameObject.Instantiate(prefab,pos,Quaternion.identity);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AssetManager.Instance.LoadAssetAsync<GameObject>("weapon", "UMP-45", 
                (o) => { GameObject.Instantiate(o, Vector3.zero, Quaternion.identity); });
        }
        //StartCoroutine(AbAsync("weapon", "UMP-45", Vector3.zero));
    }
}
