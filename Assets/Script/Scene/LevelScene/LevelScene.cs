using MyScene;
using UnityEngine;

public class LevelScene : BaseScene
{
    LevelSceneInfo sceneInfo;
    public LevelScene(string levelName,LevelSceneInfo sceneInfo) : base(levelName)
    {
        this.sceneInfo = sceneInfo;
    }
    public override void OnSceneLoaded()
    {
        if(sceneInfo!=null)
        {
            Debug.Log(sceneInfo.maxEnemyNum);
        }
    }

}
