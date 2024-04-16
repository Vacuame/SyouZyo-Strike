

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grid<TGridObj> {

    public UnityAction<int, int> onGridObjectChanged;

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObj[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObj>, int, int, TGridObj> createGridObject) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObj[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    Debug.DrawLine(GetTransformPosition(x, y), GetTransformPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetTransformPosition(x, y), GetTransformPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetTransformPosition(0, height), GetTransformPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetTransformPosition(width, 0), GetTransformPosition(width, height), Color.white, 100f);

            onGridObjectChanged += (int x,int y) => {
                debugTextArray[x, y].text = gridArray[x, y]?.ToString();
            };
        }
    }

    #region GetData
    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }
    #endregion

    #region Position
    public Vector3 GetTransformPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 relativePosition, out int x, out int y) {
        x = Mathf.FloorToInt((relativePosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((relativePosition - originPosition).y / cellSize);
    }

    public Vector2Int GetGridPosition(Vector2 anchoredPostion)
    {
        GetXY(anchoredPostion,out int x,out int y);
        return new Vector2Int(x,y);
    }
    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        int x = gridPosition.x;
        int y = gridPosition.y;

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region GridObject
    public void SetGridObject(int x, int y, TGridObj value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
            TriggerGridObjectChanged(x, y);
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        onGridObjectChanged?.Invoke(x,y);
    }

    public void SetGridObject(Vector3 worldPosition, TGridObj value) {
        GetXY(worldPosition, out int x, out int y);
        SetGridObject(x, y, value);
    }

    public TGridObj GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        } else {
            return default(TGridObj);
        }
    }

    public TGridObj GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    #endregion

}
