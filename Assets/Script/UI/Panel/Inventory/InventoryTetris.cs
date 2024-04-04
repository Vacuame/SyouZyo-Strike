using MoleMole;
using System.Collections.Generic;
using UnityEngine;
using static InventoryItem_SO;
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
    public InventoryItem_SO testSO;
    public Dir testDir;
    public Transform visual;

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

        //预览
        Vector3 placedObjectWorldPosition =
                grid.GetWorldPosition(gridPos.x, gridPos.y) +
               (Vector3)testSO.GetRotationOffset(testDir) * grid.GetCellSize() / 2;
        visual.transform.position  = itemContainer.position + placedObjectWorldPosition;


        if (Input.GetMouseButtonDown(0)) 
        {
            TryPlaceItem(testSO, gridPos, testDir);
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

    public bool TryPlaceItem(InventoryItem_SO itemSo,Vector2Int gridPos,Dir dir)
    {
        List<Vector2Int> gridPositionList = itemSo.GetGridPositionList(gridPos, dir);

        bool canPlace= true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
            {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanPlaceItem())
            {
                canPlace = false;
                break;
            }
        }

        if (canPlace)
        {
            Vector3 placedObjectWorldPosition = 
                grid.GetWorldPosition(gridPos.x, gridPos.y) +
               (Vector3)itemSo.GetRotationOffset(dir) * grid.GetCellSize()/2;

            TetrisItem itemObject = TetrisItem.Instantiate(itemSo, itemContainer, placedObjectWorldPosition, gridPos, dir, this);
            itemObject.transform.rotation = Quaternion.Euler(0, 0, -itemSo.GetRotationAngle(dir));

            //给它注册Drag功能
            //itemObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetItem(itemObject);
            }
            return true;
        }

        return false;
    }

    public void RemoveItemAt(Vector2Int gridPos)
    {
        TetrisItem item = grid.GetGridObject(gridPos.x, gridPos.y).GetItem();

        if (item != null)
        {
            List<Vector2Int> gridPositionList = item.GetGridPositionList();
            foreach (Vector2Int pos in gridPositionList)
            {
                grid.GetGridObject(pos.x, pos.y).SetItem(null);
            }

            Destroy(item.gameObject);
        }
    }
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

    public bool CanPlaceItem()
    {
        return item == null;
    }
}