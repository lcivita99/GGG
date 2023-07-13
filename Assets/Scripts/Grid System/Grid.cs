using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int width;
    public int height;
    public float cellSize;

    public bool[,] placeable;

    public Grid (int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        placeable = new bool[width, height];

        for (int x = 0; x < placeable.GetLength(0); x++)
        {
            for (int y = 0; y < placeable.GetLength(1); y++)
            {
                placeable[x, y] = true;
            }
        }
    }

    public Vector3 GetWorldPosition (int x, int y)
    {
        return new Vector3(x, y) * cellSize;
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

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.RoundToInt(worldPosition.x / cellSize);
        y = Mathf.RoundToInt(worldPosition.y / cellSize);
    }
}
