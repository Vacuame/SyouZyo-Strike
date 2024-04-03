using MoleMole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPanel : BasePanel
{
    public static readonly UIType uiType = new UIType("BagPanel");
    [SerializeField] private RectTransform itemContainer;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform backgroundBlockTemplate;
    private Grid<ItemBlock> grid;
    public int width,height;
    public float cellSize;
    protected override void Init()
    {
        grid = new Grid<ItemBlock>(width, height,cellSize, Vector3.zero, (Grid<ItemBlock> g, int x, int y) => new ItemBlock(g, x, y));
        DrawBackground();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3 screenPoint = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(itemContainer, screenPoint, null, out Vector2 anchoredPosition);
            Vector2Int gridPos = grid.GetGridPosition(anchoredPosition);
            Debug.Log(gridPos);
        }
    }

    private void DrawBackground()
    {
        background.position = itemContainer.position;
        for (float x = 0; x < width; x++)
        {
            for (float y = 0; y < height; y++)
            {
                Transform backgroundSingleTransform = Instantiate(backgroundBlockTemplate, background);
                backgroundSingleTransform.localPosition = new Vector3((x + 0.5f) * cellSize, (y + 0.5f) * cellSize, 0);
                backgroundSingleTransform.gameObject.SetActive(true);
            }
        }
    }

}

public class ItemBlock
{
    private Grid<ItemBlock> grid;
    private int x;
    private int y;
public ItemBlock(Grid<ItemBlock> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
}
