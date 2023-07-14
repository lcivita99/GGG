using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersAndTeams : MonoBehaviour
{
    public static PlayersAndTeams instance { get; set; }

    public List<GameObject> team1 = new List<GameObject>();
    public List<GameObject> team2 = new List<GameObject>();

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
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("p1") != null)
        {
            team1.Add(GameObject.FindGameObjectWithTag("p1"));
        }
        if (GameObject.FindGameObjectWithTag("p2") != null)
        {
            team2.Add(GameObject.FindGameObjectWithTag("p2"));
        }
        if (GameObject.FindGameObjectWithTag("p3") != null)
        {
            team1.Add(GameObject.FindGameObjectWithTag("p3"));
        }
        if (GameObject.FindGameObjectWithTag("p4") != null)
        {
            team2.Add(GameObject.FindGameObjectWithTag("p4"));
        }
    }
}
