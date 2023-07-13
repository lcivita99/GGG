using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance { get; private set; }

    public Grid grid;

    [SerializeField] private GameObject coinPrefab;

    void Awake()
    {
        // Singleton!
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        grid = new Grid(34, 15, 1);

        for (int x = 0; x < grid.placeable.GetLength(0); x++)
        {
            for (int y = 0; y < grid.placeable.GetLength(1); y++)
            {
                //Instantiate(coinPrefab, grid.GetWorldPosition(x, y), Quaternion.identity);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(grid.GetWorldPosition(0, grid.height), grid.GetWorldPosition(grid.width, grid.height), Color.white, 100f);
        Debug.DrawLine(grid.GetWorldPosition(grid.width, 0), grid.GetWorldPosition(grid.width, grid.height), Color.white, 100f);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int x;
            int y;
            grid.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition), out x, out y);
            InstantiatePrefab(coinPrefab, x, y);
            //Instantiate(coinPrefab, grid.GetWorldPosition(x, y), Quaternion.identity);
        }
    }

    public void InstantiatePrefab(GameObject prefab, int gridX, int gridY)
    {
        if (grid.placeable[gridX, gridY])
        {
            Instantiate(prefab, grid.GetWorldPosition(gridX, gridY), Quaternion.identity);
            grid.SetPlaceableValue(gridX, gridY, false);
        }
        // TODO bigger objects
    }

    public void RemoveFromGrid(Vector2 position)
    {
        int x;
        int y;
        grid.GetXY(position, out x, out y);
        grid.SetPlaceableValue(x, y, true);
    }

    
}
