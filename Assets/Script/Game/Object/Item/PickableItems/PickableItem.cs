using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : InteractableObj
{
    public static readonly string prefabPath = "Prefab/Item/Interact/PickableItem/";
    [SerializeField] private ItemInfo itemInfo;
    [SerializeField] private float edgeWidth;
    [SerializeField] private List<Renderer> renders;
    private Material highLightMat;
    private List<Material[]> mats = new List<Material[]>();
    private List<Material[]> edgeMats = new List<Material[]>();
    protected virtual ExtraSave extraSet { get => null; }
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
    public override void BeInteracted(PlayerCharacter character)
    {
        PlayerController controller = character.controller as PlayerController;
        controller.GetNewItem(itemInfo, extraSet);
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
    }

}
