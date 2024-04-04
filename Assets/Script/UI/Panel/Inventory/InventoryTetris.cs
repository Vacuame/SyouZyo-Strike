using MoleMole;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static TetrisItem_SO;
using static UnityEditor.Progress;

public class InventoryTetris : MonoBehaviour
{
    [SerializeField] private RectTransform itemContainer;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform backgroundBlockTemplate;
    [HideInInspector]public InventoryPanel inventoryPanel;
    private Grid<ItemBlock> grid;
    public int width,height;
    public float cellSize;

    #region Get
    public RectTransform GetItemContainer() { return itemContainer; }
    public Grid<ItemBlock> GetGrid() {  return grid; }
    #endregion

    [Header("DEBUG")]
    public TetrisItem_SO testSO;
    public Dir testDir;

    public void Init(InventoryPanel panel)
    {
        inventoryPanel = panel;
        grid = new Grid<ItemBlock>(width, height,cellSize, Vector3.zero, (Grid<ItemBlock> g, int x, int y) => new ItemBlock(g, x, y));
        DrawBackground();
    }

    private void Update()
    {
        Vector3 screenPoint = Input.mousePosition + inventoryPanel.inventoryDrager.mouseGridOffset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(itemContainer, screenPoint, null, out Vector2 anchoredPosition);
        Vector2Int gridPos = grid.GetGridPosition(anchoredPosition);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            testDir = inventoryPanel.inventoryDrager.GetDir();
            TryPlaceNewItem(testSO, gridPos, testDir);
        }
        if(Input.GetMouseButtonDown(1)) 
        {
            RemoveItemAt(gridPos);
        }
    }

    private void DrawBackground()
    {
        background.position = itemContainer.position;
        for (float x = 0; x < width; x++)
        {
            for (float y = 0; y < height; y++)
            {
                Transform backgroundBlockTransform = Instantiate(backgroundBlockTemplate, background);
                backgroundBlockTransform.localPosition = new Vector3((x + 0.5f) * cellSize, (y + 0.5f) * cellSize, 0);
                RectTransform rect = (RectTransform)backgroundBlockTransform;
                rect.sizeDelta = new Vector2(cellSize, cellSize);
                backgroundBlockTransform.gameObject.SetActive(true);
            }
        }
    }

    public bool TryPlaceNewItem(TetrisItem_SO itemSO,Vector2Int gridPos,Dir dir)
    {
        if (CanPlaceNew(itemSO, gridPos, dir))
        {
            PlaceNewItem(itemSO, gridPos, dir);
            return true;
        }
        return false;
    }
    public void PlaceNewItem(TetrisItem_SO itemSO, Vector2Int gridPos, Dir dir)
    {
        Transform itemObject = Instantiate(itemSO.prefab);
        TetrisItem item = itemObject.GetComponent<TetrisItem>();
        item.item_SO = itemSO;
        item.SetTetris(gridPos, dir, this);
        itemObject.transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(dir));

        SetGridItem(item,gridPos,dir);
    }
    public bool CanPlaceNew(TetrisItem_SO itemSO, Vector2Int gridPos, Dir dir)
    {
        List<Vector2Int> gridPositionList = itemSO.GetGridPositionList(gridPos, dir);
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
                return false;
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).Empty())
                return false;
        }
        return true;
    }
    public void RemoveItemAt(Vector2Int gridPos)
    {
        TetrisItem item = grid.GetGridObject(gridPos.x, gridPos.y).GetItem();

        if (item != null)
        {
            ClearGridItem(item);
            Destroy(item.gameObject);
        }
    }
    public bool CanDragTo(TetrisItem item, Vector2Int gridPos, Dir dir)
    {
        List<Vector2Int> gridPositionList = item.item_SO.GetGridPositionList(gridPos, dir);
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
                return false;

            ItemBlock block = grid.GetGridObject(gridPosition.x, gridPosition.y);
            if (!block.Empty()&& block.GetItem()!=item)
                return false;
        }
        return true;
    }

    #region Grid
    public void SetGridItem(TetrisItem item, Vector2Int gridPos, Dir dir)
    {
        foreach (Vector2Int gridPosition in item.item_SO.GetGridPositionList(gridPos, dir))
            grid.GetGridObject(gridPosition.x, gridPosition.y).SetItem(item);
    }
    public void ClearGridItem(TetrisItem item)
    {
        List<Vector2Int> gridPositionList = item.GetGridPositionList();
        foreach (Vector2Int pos in gridPositionList)
        {
            grid.GetGridObject(pos.x, pos.y).SetItem(null);
        }
    }
    public Vector2Int GetGridPosInScreen(Vector3 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (GetItemContainer(), screenPos, null, out Vector2 anchoredPosition);
        return grid.GetGridPosition(anchoredPosition);
    }
    #endregion
}

public class ItemBlock
{
    private Grid<ItemBlock> grid;
    private int x;
    private int y;
    private TetrisItem item;
    public TetrisItem GetItem()
    {
        return item;
    }
    public void SetItem(TetrisItem item)
    {
        this.item = item;
    }

    public ItemBlock(Grid<ItemBlock> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public bool Empty()
    {
        return item == null;
    }
}