using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumSlotManager : SlotManager
{

    public static MediumSlotManager instance { get; private set; }

    public List<PlayerInteractionManager> playerInteractionManagers = new List<PlayerInteractionManager>();
    // add in editor
    public List<GameObject> mediumCribmates = new List<GameObject>();

    //Dictionary of CribIDs with their respective stats
    public Dictionary<int, CribmateStats> cribmateDictionary = new Dictionary<int, CribmateStats>();


    // List of cribmate distribution through time
    private List<int> originalProbabilities;
    private List<int> secondWave;

    //weighted Probs
    public List<int> weightedProbabilities;

    // slot positions and position list
    public Vector2 slotPosition;



    // Cribmate Currently in the Shop
    public GameObject currentCribmate;

    private int cribID;

    //List<(int, int)> cribAndSlotList = new List<(int, int)>();


    // add the cribmates:

    CribmateStats cribmate0Stats = new CribmateStats
    {
        name = "medium03",
        cribID = 0,
        poolOdds = 3,
        cost = 10,
        slot = 1
    };

    CribmateStats cribmate1Stats = new CribmateStats
    {
        name = "medium01",
        cribID = 1,
        poolOdds = 3,
        cost = 10,
        slot = 1
    };

    CribmateStats cribmate2Stats = new CribmateStats
    {
        name = "medium02",
        cribID = 2,
        poolOdds = 3,
        cost = 10,
        slot = 1
    };

    CribmateStats cribmate3Stats = new CribmateStats
    {
        name = "medium03",
        cribID = 3,
        poolOdds = 3,
        cost = 10,
        slot = 1
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

        slotPosition = new Vector2(0f, 5.9f);
        //currentCribmate = Instantiate(mediumCribmates[0], slotPosition, Quaternion.identity);
        //cribID = 1;

    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("p1") != null)
        {
            playerInteractionManagers.Add(GameObject.FindGameObjectWithTag("p1").GetComponent<PlayerInteractionManager>());
        }
        if (GameObject.FindGameObjectWithTag("p2") != null)
        {
            playerInteractionManagers.Add(GameObject.FindGameObjectWithTag("p2").GetComponent<PlayerInteractionManager>());
        }
        if (GameObject.FindGameObjectWithTag("p3") != null)
        {
            playerInteractionManagers.Add(GameObject.FindGameObjectWithTag("p3").GetComponent<PlayerInteractionManager>());
        }
        if (GameObject.FindGameObjectWithTag("p4") != null)
        {
            playerInteractionManagers.Add(GameObject.FindGameObjectWithTag("p4").GetComponent<PlayerInteractionManager>());
        }

        int index = Random.Range(0, originalProbabilities.Count);
        int picked = originalProbabilities[index];

        GameObject instance = Instantiate(mediumCribmates[picked], slotPosition, Quaternion.identity);
        currentCribmate = instance;
        currentCribmate.GetComponent<CribmateManager>().SetStats(cribmateDictionary[picked]);
        cribID = picked;
        originalProbabilities.Remove(picked);
        AddCurrentCribmateToAllDictionaries();

        cribID = picked;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeSlot();
        }
    }

    public override void ChangeSlot()
    {
        RemoveCurrentCribmateFromAllDictionaries();

        Instantiate(currentCribmate.GetComponent<CribmateManager>().deathAnimPrefab, slotPosition, Quaternion.identity);

        Destroy(currentCribmate);
        weightedProbabilities = new List<int>(originalProbabilities);
        weightedProbabilities.RemoveAll(item => item == cribID);

        if (weightedProbabilities.Count == 0)
        {
            ChangePool();
            weightedProbabilities.RemoveAll(item => item == cribID);
        }


        int index = Random.Range(0, weightedProbabilities.Count);
        int picked = weightedProbabilities[index];

        GameObject instance = Instantiate(mediumCribmates[picked], slotPosition, Quaternion.identity);
        currentCribmate = instance;
        currentCribmate.GetComponent<CribmateManager>().SetStats(cribmateDictionary[picked]);
        cribID = picked;
        originalProbabilities.Remove(picked);
        AddCurrentCribmateToAllDictionaries();
    }

    private void AddCurrentCribmateToAllDictionaries()
    {
        for (int i = 0; i < playerInteractionManagers.Count; i++)
        {
            playerInteractionManagers[i].AddInteractableObj(currentCribmate);
        }
    }

    private void RemoveCurrentCribmateFromAllDictionaries()
    {
        for (int i = 0; i < playerInteractionManagers.Count; i++)
        {
            playerInteractionManagers[i].RemoveInteractableObj(currentCribmate);
        }
    }


    private void DestroyPreviousCrib()

    {

    }

    private void ChangePool()
    {
        originalProbabilities = new List<int>(secondWave);
        weightedProbabilities = originalProbabilities;
    }
}
