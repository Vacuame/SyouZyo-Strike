using Unity.VisualScripting;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    protected virtual void Awake()
    {
        GameRoot.Instance.gameMode = this;
    }
    
    protected virtual void OnDestroy()
    {
        if(GameRoot.Instance.gameMode == this)
        {
            GameRoot.Instance.gameMode = null;
        }
    }

}
