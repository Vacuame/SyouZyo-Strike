using MyScene;
using MyUI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifficultySelectPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("DifficultySelect/DifficultySelectPanel");

    public List<DifficultyButtonInfo> difficultyBtns = new List<DifficultyButtonInfo>();
    public Text txtIntro;

    [System.Serializable]
    public class DifficultyButtonInfo
    {
        public Button btn;
        [TextArea]
        public string intro;
        public GameModeInfo gameModeInfo;
    }

    protected override void Init()
    {
        foreach(var difBtn in difficultyBtns)
        {
            Button button = difBtn.btn;

            button.onClick.AddListener(() => SceneSystem.Instance.SetSceneAsync(
                new LevelScene("Level1", difBtn.gameModeInfo)));

            EventTrigger eventTrigger = button.gameObject.GetOrAddComponent<EventTrigger>();

            // 鼠标悬停事件
            EventTrigger.Entry eventEntry = new EventTrigger.Entry();
            eventEntry.eventID = EventTriggerType.PointerEnter;
            eventEntry.callback.AddListener((data) => txtIntro.text = difBtn.intro);
            eventTrigger.triggers.Add(eventEntry);

            // 鼠标离开事件
            eventEntry = new EventTrigger.Entry();
            eventEntry.eventID = EventTriggerType.PointerExit;
            eventEntry.callback.AddListener((data) => txtIntro.text = "" );
            eventTrigger.triggers.Add(eventEntry);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            UIManager.Instance.Pop();
        }
    }

}
