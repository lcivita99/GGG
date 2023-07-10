using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // Singleton Variable
    public static ShopManager instance { get; private set; }

    // List of all cribmates
    public List<GameObject> cribmates = new List<GameObject>();

    //Dictionary of CribIDs with their respective stats
    public Dictionary<int, CribmateStats> cribmateDictionary = new Dictionary<int, CribmateStats>();


    // List of cribmate distribution through time
    private List<int> originalProbabilities;
    private List<int> secondWave;

    //weighted Probs
    List<int> weightedProbabilities;


    // amount of cribmates that will be swapped
    private int cribsToSwap;

    // slot positions and position list
    public Vector2 firstSlotPosition;
    public Vector2 secondSlotPosition;
    public Vector2 thirdSlotPosition;
    public List<Vector2> slots = new List<Vector2>();


    // List of Cribmates currently in the shop
    public List<GameObject> curShopCribmates = new List<GameObject>(3);

    private int[] cribList = new int[3];

    //List<(int, int)> cribAndSlotList = new List<(int, int)>();


    // add the cribmates:

    CribmateStats cribmate0Stats = new CribmateStats
    {
        name = "jacque",
        cribID = 0,
        poolOdds = 3,
        cost = 10
    };

    CribmateStats cribmate1Stats = new CribmateStats
    {
        name = "luca",
        cribID = 1,
        poolOdds = 3,
        cost = 10
    };

    CribmateStats cribmate2Stats = new CribmateStats
    {
        name = "pedro",
        cribID = 2,
        poolOdds = 3,
        cost = 10
    };

    CribmateStats cribmate3Stats = new CribmateStats
    {
        name = "luna",
        cribID = 3,
        poolOdds = 3,
        cost = 10
    };


    private void Awake()
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
    }

    private void Update()
    {

    }

    


}
