using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int width;
    public int height;
    public float cellSize;
    public Vector2 originPosition;

    public bool[,] placeable;

    public Grid (int width, int height, float cellSize, Vector2 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        placeable = new bool[width, height];

        for (int x = 0; x < placeable.GetLength(0); x++)
        {
            for (int y = 0; y < placeable.GetLength(1); y++)
            {
                placeable[x, y] = true;
            }
        }
    }

    public Vector2 GetWorldPosition (int x, int y)
    {
        return new Vector2(x, y) * cellSize + originPosition;
    }

    public void SetPlaceableValue(int x, int y, bool value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            placeable[x, y] = value;
        }
        else
        {
            Debug.Log("RED ALERT - you passed in illegal values");
        }
    }

    public void GetXY(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.RoundToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.RoundToInt((worldPosition - originPosition).y / cellSize);
    }
}
