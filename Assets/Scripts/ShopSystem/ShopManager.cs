using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // list of cribmates

    public static ShopManager instance { get; private set; }

    public List<GameObject> cribmates = new List<GameObject>();

    public Dictionary<int, CribmateStats> cribmateDictionary = new Dictionary<int, CribmateStats>();


    private List<int> originalProbabilities;
    private List<int> secondWave;



    private int cribsToSwap;

    private int[] cribIDs = new int[3] { 0, 1, 2 };

    public Vector2 firstSlotPosition;
    public Vector2 secondSlotPosition;
    public Vector2 thirdSlotPosition;

    public List<Vector2> slots = new List<Vector2>();

    public List<GameObject> curShopCribmates = new List<GameObject>();


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

        originalProbabilities = new List<int> { 0, 0, 0, 0, 1, 1, 1, 2, 2, 3 };

        secondWave = new List<int> { 0, 1, 1, 1, 2, 2, 3, 3, 3, 3 };

        cribmateDictionary[0] = cribmate0Stats;
        cribmateDictionary[1] = cribmate1Stats;
        cribmateDictionary[2] = cribmate2Stats;
        cribmateDictionary[3] = cribmate3Stats;



        firstSlotPosition = new Vector2(-2.3f, 6.69f);
        secondSlotPosition = new Vector2(-0, 6.69f);
        thirdSlotPosition = new Vector2(2.3f, 6.69f);

        slots.Add(firstSlotPosition);
        slots.Add(secondSlotPosition);
        slots.Add(thirdSlotPosition);

        AddInitialShop();

        //FillShop(3);
       // DisplayThreeCribMates();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("Swapping");
            FillShop();
        }
    }

    public void FillShop ()
    {
        List<(int, int)> cribAndSlotList = new List<(int, int)>();
        // Destroy the Cribmates that have to be swapped and update their location
        DestroyShopCribmates(cribAndSlotList);

        // initialized weighted list
        List<int> weightedProbabilities = new List<int>(originalProbabilities);

        // list that show the picked number
        List<int> pickedNumbers = new List<int>();

        HashSet<int> uniqueCribsInPool = new HashSet<int>(weightedProbabilities);


        if (cribsToSwap > uniqueCribsInPool.Count)
        {
            originalProbabilities = new List<int>(secondWave);
            weightedProbabilities = new List<int>(secondWave);
        }



        for (int i = 0; i < cribsToSwap; i++) // pick x numbers
       {

            int index = Random.Range(0, weightedProbabilities.Count);
            int picked = weightedProbabilities[index];

            // removes from the original pool:
            RemoveFromPool(index);

            // ENSURE PICK is not the same as previous
            if (picked == cribAndSlotList[i].Item1)
            {
                Debug.Log("SAME");
            }


            // Remove all occurrences of the picked number to ensure uniqueness
            weightedProbabilities.RemoveAll(item => item == picked);
            var provtuple = cribAndSlotList[i];
            cribAndSlotList[i] = (picked, provtuple.Item2);

            
        }

        cribsToSwap = 0;

        // Add the cribmates to
        for (int i = 0; i < cribAndSlotList.Count; i++)
        {
            
            GameObject instance = Instantiate(cribmates[cribAndSlotList[i].Item1], slots[cribAndSlotList[i].Item2], Quaternion.identity);
            instance.GetComponent<CribmateManager>().SetStats(cribmateDictionary[cribAndSlotList[i].Item1]);
            curShopCribmates.Add(instance);
        }      
    }


    private void AddInitialShop()
    {
        // Add the cribmates to
        for (int i = 0; i < 3; i++)
        {
            GameObject instance = Instantiate(cribmates[i], slots[i], Quaternion.identity);
            instance.GetComponent<CribmateManager>().SetStats(cribmateDictionary[i]);
            curShopCribmates.Add(instance);
        }
    }

    public void UpdatePool() { }

    private void DestroyShopCribmates(List<(int, int)> pickedCribs)
    {
        // Create a copy of the curShopCribmates list for iteration
        List<GameObject> curShopCribmatesCopy = new List<GameObject>(curShopCribmates);
        List<GameObject> cribsToDestroy = new List<GameObject>();

        for (int i = 0; i < curShopCribmatesCopy.Count; i++)
        {

            CribmateManager curCribmate = curShopCribmatesCopy[i].GetComponent<CribmateManager>();

            if (curCribmate.stats.swap)
            {
                cribsToSwap++;
                pickedCribs.Add((curCribmate.stats.cribID, i));
                GameObject cribDestroy = curShopCribmatesCopy[i];
                //cribIDs[i] = curShopCribmatesCopy[i].GetComponent<CribmateManager>().stats.cribID;
                cribsToDestroy.Add(cribDestroy);
            }
        }

        // Destroying and removing GameObjects from the original list
        foreach (GameObject crib in cribsToDestroy)
        {
            curShopCribmates.Remove(crib);
            Destroy(crib);
        }
    }


    private void RemoveFromPool(int index)
    {
        originalProbabilities.RemoveAt(index);
    }
}
