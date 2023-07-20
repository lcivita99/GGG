using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public int teamID;

    private HashSet<CombatStateManager> subscribedObjects = new HashSet<CombatStateManager>();


    private void OnTriggerEnter2D(Collider2D collision)
    {



        if (collision.gameObject.layer.Equals(3))
        {
            // Check if the collided object has the YourScriptWithGetHit component.
            CombatStateManager CSM = collision.GetComponent<CombatStateManager>();
            if (CSM != null && !subscribedObjects.Contains(CSM))
            {
                // Subscribe to the OnHit event when the trigger collision occurs.
                CSM.OnHit += OnGetHit;
                subscribedObjects.Add(CSM);
            }


            if (teamID == 1) // if its team1's flag
            {
                if (collision.gameObject.tag == "p1" || collision.gameObject.tag == "p3")
                {
                    GetFlag(collision.gameObject);
                }
            }
            else if (teamID == 2) // if its team2's flag
            {
                if (collision.gameObject.tag == "p2" || collision.gameObject.tag == "p4")
                {
                    GetFlag(collision.gameObject);
                }
            }
        }
    }

    private void OnGetHit(Collider2D collision)
    {
        Debug.Log("I got hit");
    }


    private void GetFlag(GameObject gm)
    {
        Debug.Log(gm.tag);
        gameObject.transform.position = Vector2.zero;
    }
}
