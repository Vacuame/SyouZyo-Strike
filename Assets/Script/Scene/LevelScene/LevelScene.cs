using MyScene;
using UnityEngine;

public class LevelScene : BaseScene
{
    GameModeInfo gameModeInfo;
    public LevelScene(string levelName, GameModeInfo gameModeInfo) : base(levelName)
    {
        this.gameModeInfo = gameModeInfo;
    }
    public override void OnSceneLoaded()
    {
        if(gameModeInfo!=null)
        {
            (GameRoot.Instance.gameMode as GameMode_Play).LoadGameModeInfo(gameModeInfo);
        }
    }

}
