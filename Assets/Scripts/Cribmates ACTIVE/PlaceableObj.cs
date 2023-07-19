using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObj : MonoBehaviour
{
    public int myTeam;

    [HideInInspector] public List<GameObject> enemyObjs = new List<GameObject>();
    [HideInInspector] public List<GameObject> allyObjs = new List<GameObject>();

    [HideInInspector] public List<CombatStateManager> enemyCSMs = new List<CombatStateManager>();
    //[HideInInspector] public List<GameObject> allyObjs = new List<GameObject>();

    [HideInInspector] public PlayersAndTeams playersAndTeams;

    private void Start()
    {
    }

    // must call whenever instantiating
    public void SetTeam(int team)
    {
        playersAndTeams = PlayersAndTeams.instance;
        myTeam = team;

        if (myTeam == 1)
        {
            for (int i = 0; i < playersAndTeams.team2.Count; i++)
            {
                enemyObjs.Add(playersAndTeams.team2[i]);
                enemyCSMs.Add(playersAndTeams.team2[i].GetComponent<CombatStateManager>());
                allyObjs.Add(playersAndTeams.team1[i]);
            }
        }
        else if (myTeam == 2)
        {
            for (int i = 0; i < playersAndTeams.team1.Count; i++)
            {
                enemyObjs.Add(playersAndTeams.team1[i]);
                enemyCSMs.Add(playersAndTeams.team1[i].GetComponent<CombatStateManager>());
                allyObjs.Add(playersAndTeams.team2[i]);
            }
        }
    }
}
