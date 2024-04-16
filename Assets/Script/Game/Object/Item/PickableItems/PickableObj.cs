using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickableObj : InteractableObj
{
    public static readonly string prefabPath = "Prefab/Item/Interact/PickableItem/";
    [SerializeField] private ItemInfo itemInfo;
    [SerializeField] private float edgeWidth;
    [SerializeField] private List<Renderer> renders;
    private Material highLightMat;
    private List<Material[]> mats = new List<Material[]>();
    private List<Material[]> edgeMats = new List<Material[]>();

    #region ¹âÖù
    [HideInInspector]public GameObject lightColumn;
    [SerializeField] private float lightIntensity_fade,lightIntensity_highLight;
    [SerializeField]private Color lightColumnColor_fade, lightColumnColor_highLight;
    private Renderer lightRender;
    public void AddLightColum()
    {
        lightColumn = GameObject.Instantiate(Resources.Load<GameObject>("Effect/LightColumn"), transform.position, Quaternion.identity);
        lightColumn.transform.SetParent(transform, true);
        lightRender = lightColumn.GetComponentInChildren<Renderer>();
        SetLightColumColor(lightColumnColor_fade, lightIntensity_fade);
    }
    private void SetLightColumColor(Color color,float lightIntensity)
    {
        Material lightMat = new Material(lightRender.material);
        Color.RGBToHSV(color, out float _h, out float _s, out float _v);
        lightMat.SetColor("_EmissionColor", Color.HSVToRGB(_h, _s, lightIntensity));
        lightRender.material = lightMat;
    }
    #endregion

    public virtual ExtraSave extraSet { get; set; }
    private void Awake()
    {
        highLightMat = Resources.Load<Material>("Material/EdgeGlow");
        highLightMat = new Material(highLightMat);
        highLightMat.SetFloat("_Outline", edgeWidth);
        foreach(var render in renders)
        {
            mats.Add(new Material[1] { render.material });
            edgeMats.Add(new Material[2] { render.material, highLightMat });
        }
    }
    public override void BeInteracted(PlayerCharacter character, UnityAction onInteractOver)
    {
        PlayerController controller = character.controller as PlayerController;
        controller.PickUpNewItem(itemInfo, extraSet);
        onInteractOver?.Invoke();
        Destroy(gameObject);
    }

    public override void SetSelected(bool isSelected)
    {
        base.SetSelected(isSelected);
        List<Material[]> materials = isSelected? edgeMats:mats;
        for (int i = 0; i < renders.Count; i++)
        {
            if (renders[i] != null)
                renders[i].materials = materials[i];
        }

        if(lightColumn!=null)
        {
            Color lightColumnColor = isSelected ? lightColumnColor_highLight : lightColumnColor_fade;
            float lightIntensity = isSelected ? lightIntensity_highLight : lightIntensity_fade;
            SetLightColumColor(lightColumnColor, lightIntensity);
        }

        if (isSelected)
            HUDManager.GetHUD<PickUpHUD>().SetPickUpTip(transform, itemInfo.itemName, extraSet.num);
        else
            HUDManager.GetHUD<PickUpHUD>().HidePickUpTip();

    }

}
