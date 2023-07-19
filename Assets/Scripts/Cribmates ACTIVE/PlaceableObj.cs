using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlaceableObj : MonoBehaviour
{
    public int myTeam;

    public int curHealth;
    public int maxHealth;

    public CombatStateManager placer;

    [HideInInspector] public List<GameObject> enemyObjs = new List<GameObject>();
    [HideInInspector] public List<GameObject> allyObjs = new List<GameObject>();

    [HideInInspector] public List<CombatStateManager> enemyCSMs = new List<CombatStateManager>();
    //[HideInInspector] public List<GameObject> allyObjs = new List<GameObject>();

    [HideInInspector] public PlayersAndTeams playersAndTeams;

    private void Start()
    {
    }

    // must call whenever instantiating
    public void SetTeam(int team, CombatStateManager combat)
    {
        placer = combat;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(6))
        {
            curHealth -= 1;
        } else if (collision.gameObject.layer.Equals(7))
        {
            curHealth -= 2;
            Debug.Log(curHealth);
        }

        if (curHealth <= 0)
        {
            Bounds bounds = GetComponent<Collider2D>().bounds;
            var guo = new GraphUpdateObject(bounds);
            GridManager.instance.RemoveFromGrid(transform.position);

            Destroy(gameObject);
            // update pathfinding

            // Set some settings
            guo.updatePhysics = true;
            //guo.updatePhysics = false;
            AstarPath.active.UpdateGraphs(guo);
            //free from grid

           
        }
    }
}
