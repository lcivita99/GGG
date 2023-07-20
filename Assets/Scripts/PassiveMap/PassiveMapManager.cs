using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveMapManager : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject heart;

    private float coinTimer;
    private float timeToNextCoin = 5f;
    private float maxCoinTime = 7f;
    private float minCoinTime = 3f;

    private float heartTimer;
    private float timeToNextHeart = 15f;
    private float maxHeartTime = 20f;
    private float minHeartTime = 10f;

    private GridManager gridManager;
    void Start()
    {
        gridManager = GridManager.instance;
    }

    void Update()
    {
        coinTimer += Time.deltaTime;
        heartTimer += Time.deltaTime;

        if (coinTimer >= timeToNextCoin)
        {
            Spawn(coin, minCoinTime, maxCoinTime, out timeToNextCoin);
            Spawn(coin, minCoinTime, maxCoinTime, out timeToNextCoin);
            coinTimer = 0;
        }
        
        // else if so that we avoid running them on same frame? Idk if necessary, just safety
        else if (heartTimer >= timeToNextHeart)
        {
            Spawn(heart, minHeartTime, maxHeartTime, out timeToNextHeart);
            heartTimer = 0;
        }
    }

    private void Spawn(GameObject prefab, float min, float max, out float timeToNext)
    {
        Vector2 pos = gridManager.grid.availableSpots[Random.Range(0, gridManager.grid.availableSpots.Count - 1)];
       // Debug.Log(pos);
        gridManager.InstantiatePrefab(prefab, Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        timeToNext = Random.Range(min, max);
    }
}
