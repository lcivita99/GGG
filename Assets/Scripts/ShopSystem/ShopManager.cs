using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // list of cribmates

    public List<GameObject> cribmates = new List<GameObject>();

    public Dictionary<int, CribmateStats> cribmateDictionary = new Dictionary<int, CribmateStats>();


    private List<int> originalProbabilities;


    private int cribsToSwap;

    public Vector2 firstSlotPosition;
    public Vector2 secondSlotPosition;
    public Vector2 thirdSlotPosition;

    public List<Vector2> slots = new List<Vector2>();

    public List<GameObject> curShopCribmates = new List<GameObject>();


    // add the cribmates:

    CribmateStats cribmate0Stats = new CribmateStats
    {
        name = "jacque",
        poolOdds = 3,
        cost = 10
    };

    CribmateStats cribmate1Stats = new CribmateStats
    {
        name = "luca",
        poolOdds = 3,
        cost = 10
    };

    CribmateStats cribmate2Stats = new CribmateStats
    {
        name = "pedro",
        poolOdds = 3,
        cost = 10
    };

    CribmateStats cribmate3Stats = new CribmateStats
    {
        name = "luna",
        poolOdds = 3,
        cost = 10
    };





    private void Awake()
    {

        originalProbabilities = new List<int> { 0, 0, 0, 0, 1, 1, 1, 2, 2, 3 };

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

        //FillShop(3);
       // DisplayThreeCribMates();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Swapping");
            FillShop(3);
        }
    }

    public void FillShop (int swapIndex)
    {
        // currently deletes entire shop
        for (int i = 0; i < curShopCribmates.Count; i++)
        {
            Destroy(curShopCribmates[i]);
        }
        // will always initially be a copy of the original probabilities
        List<int> weightedProbabilities = new List<int>(originalProbabilities);

        // FIRST: CHECK THE SHOPLIST:
        // TODO: update laters
        cribsToSwap = 3;
        // Second: Calculate the new cribmate to be added:

        List<int> pickedNumbers = new List<int>();
        //
        for (int i = 0; i < cribsToSwap; i++) // pick 3 numbers
       {
            if (weightedProbabilities.Count == 0)
            {
                break; // Avoid an infinite loop if there are less than 3 unique numbers
            }
            int index = Random.Range(0, weightedProbabilities.Count);
            int picked = weightedProbabilities[index];
            // Remove all occurrences of the picked number to ensure uniqueness
            weightedProbabilities.RemoveAll(item => item == picked);
            pickedNumbers.Add(picked);
        }
        //
        for (int i = 0; i < pickedNumbers.Count; i++)
        {
            Debug.Log(slots[i]);
            GameObject instance = Instantiate(cribmates[pickedNumbers[i]], slots[i], Quaternion.identity);
            curShopCribmates.Add(instance);
        }
            
    }

    // [ 1, 1, 1, 1, 2, 2, 2, 3, 3, 4]

}
