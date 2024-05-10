using MyScene;
using MyUI;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("GameOverPanel");

    public Image imgBackgroud;
    public Text txtGameOver;
    private float maxBackGroundAlpha;
    private int maxFontSize;

    private float timeSinceStart;
    public float timeToLerp;
    public float timeToSettlement;

    public Transform settlementPanelTrans;
    public Button btnReturnToMenu;

    protected override void Init()
    {
        maxBackGroundAlpha = imgBackgroud.color.a;
        maxFontSize = txtGameOver.fontSize;

        imgBackgroud.color = new Color(0, 0, 0, 0);
        txtGameOver.fontSize = 1;


        settlementPanelTrans.PanelAppearance(false);
        btnReturnToMenu.onClick.AddListener(() => SceneSystem.Instance.SetSceneAsync(
            new MainMenuScene(), needPressKey:false));
    }


    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        if(timeSinceStart > timeToSettlement)
        {
            settlementPanelTrans.PanelAppearance(true);
        }

        float alpha = Mathf.Lerp(0, maxBackGroundAlpha, timeSinceStart / timeToLerp);
        imgBackgroud.color = new Color(0, 0, 0, alpha);

        float floatTxtSize = Mathf.Lerp(0, maxFontSize, timeSinceStart / timeToLerp);
        txtGameOver.fontSize = (int)floatTxtSize;


    }

}
