using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // list of cribmates

    public List<GameObject> cribmates = new List<GameObject>();

    public Dictionary<int, CribmateStats> cribmateDictionary = new Dictionary<int, CribmateStats>();


    public List<int> originalProbabilities = new List<int>();
    // will always initially be a copy of the original probabilities
    public List<int> weightedProbabilities = new List<int>();

    private int cribsToSwap;


    // add the cribmates:
    CribmateStats cribmate1Stats = new CribmateStats
    {
        name = "luca",
        poolOdds = 3,
        cost = 10
    };

    private void Awake()
    {
        FillShop();
       // DisplayThreeCribMates();
    }

    public void FillShop ()
    {
        // FIRST: CHECK THE SHOPLIST:
        // TODO: update laters
        cribsToSwap = 3;
        // Second: Calculate the new cribmate to be added:

        // Create the weighted selection pool
        foreach (int number in originalProbabilities)
        {
            weightedProbabilities.Add(number);
        }

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
    }

    // [ 1, 1, 1, 1, 2, 2, 2, 3, 3, 4]

}
