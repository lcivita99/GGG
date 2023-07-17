using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperShot : MonoBehaviour
{
    private float sniperShieldStunLength = 0.2f;

    private Vector3 shotDir;

    [SerializeField] private Transform sniper;

    private PlayersAndTeams playersAndTeams;

    private Dictionary<GameObject, bool> hasHitEnemies = new Dictionary<GameObject, bool>();

    private int myTeam;

    private void OnEnable()
    {
        //Debug.Log("enabled");
        List<GameObject> keys = new List<GameObject>(hasHitEnemies.Keys);
        foreach (GameObject obj in keys)
        {
            hasHitEnemies[obj] = false;
        }
    }

    private void Awake()
    {
        sniper = transform.parent.parent;
        playersAndTeams = PlayersAndTeams.instance;

        myTeam = sniper.GetComponent<PlaceableObj>().myTeam;

        if (myTeam == 1)
        {
            foreach (GameObject obj in playersAndTeams.team2)
            {
                hasHitEnemies.Add(obj, false);
            }
        }
        
        else
        {
            foreach (GameObject obj in playersAndTeams.team1)
            {
                hasHitEnemies.Add(obj, false);
            }
        }
        
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(3)) // player
        {
            if (IsEnemyPlayer(collision)) // what kind of player? 
            {
                if (!hasHitEnemies[collision.gameObject])
                {
                    hasHitEnemies[collision.gameObject] = true;

                    // get combat state manager:
                    CombatStateManager combat = collision.GetComponent<CombatStateManager>();

                    //Debug.Log("entrered");
                    // calculate direction: 

                    // basically calculating explosion force
                    Vector2 dir = new Vector2(collision.transform.position.x, collision.transform.position.y) - collision.ClosestPoint(transform.position);
                    if (combat.currentState == combat.ShieldState || combat.currentState == combat.ShieldStunState)
                    {
                        combat.SwitchState(combat.ShieldStunState, sniperShieldStunLength, "", dir);
                    }
                    else
                    {
                        combat.currentState.ForcedOutOfState(combat);
                        combat.SwitchState(combat.HitstunState, 0, "sniper", dir);
                    }
                    gameObject.SetActive(false);
                }
                
            }
        }
    }

    private bool IsEnemyPlayer(Collider2D collision)
    {
        int bulletTeam = sniper.gameObject.GetComponent<PlaceableObj>().myTeam;
        int colliderTeam = collision.gameObject.GetComponent<PlayerMovement>().team;
        if (bulletTeam == colliderTeam)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
