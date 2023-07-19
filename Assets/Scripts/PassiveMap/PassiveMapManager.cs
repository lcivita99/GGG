using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveMapManager : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject heart;

    private float coinTimer;
    private float timeToNextCoin = 5f;

    private GridManager gridManager;
    void Start()
    {
        gridManager = GridManager.instance;
    }

    void Update()
    {
        coinTimer += Time.deltaTime;

        if (coinTimer >= timeToNextCoin)
        {
            Spawn(coin, 3f, 7f, out timeToNextCoin);
            coinTimer = 0;
        }
    }

    private void Spawn(GameObject prefab, float min, float max, out float timeToNext)
    {
        Vector2 pos = gridManager.grid.availableSpots[Random.Range(0, gridManager.grid.availableSpots.Count - 1)];

        gridManager.InstantiatePrefab(prefab, Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        timeToNext = Random.Range(min, max);
    }

    private void UpdateGrid()
    {

    }
}
