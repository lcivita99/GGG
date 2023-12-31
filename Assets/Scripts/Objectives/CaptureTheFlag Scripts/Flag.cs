using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public int teamID;

    private HashSet<CombatStateManager> subscribedObjects = new();

    CombatStateManager curCSM;

    private SpriteRenderer sr;
    private Collider2D flagCollider;

    private Vector2 originalPosition;

    private bool flagWithPlayer = false;

    private void OnEnable()
    {
        // Subscribe to the event when this script becomes active/enabled.
        EventMapManager.instance.EndEvent += ObjectiveEnded;
        originalPosition = gameObject.transform.position;
    }

    private void ObjectiveEnded()
    {
        // Create a list to store items to remove after iterating.
        List<CombatStateManager> itemsToRemove = new List<CombatStateManager>();

        // Unsubscribe from gethit event (prevent memory leak)
        foreach (CombatStateManager combat in subscribedObjects)
        {
            combat.HitstunState.OnHit -= OnGetHit;
            itemsToRemove.Add(combat);
        }

        // Remove the items from the subscribedObjects list.
        foreach (CombatStateManager combat in itemsToRemove)
        {
            subscribedObjects.Remove(combat);
        }

        // Unsubscribe from EndEvent (prevent memory leak)
        EventMapManager.instance.EndEvent -= ObjectiveEnded;

        gameObject.SetActive(false);

    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        flagCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.layer.Equals(6))
        {
           // Debug.Log("destroyed");
            gameObject.transform.position = originalPosition;
        }
        else if (collision.gameObject.layer.Equals(7))
        {
           // Debug.Log("destroyed");
            gameObject.transform.position = originalPosition;
        }

        



            if (collision.gameObject.layer.Equals(3))
        {
            // Check if the collided object has the YourScriptWithGetHit component.
            curCSM = collision.GetComponent<CombatStateManager>();
            if (curCSM != null && !subscribedObjects.Contains(curCSM))
            {
                // Subscribe to the OnHit event when the trigger collision occurs.
                curCSM.HitstunState.OnHit += OnGetHit;
                subscribedObjects.Add(curCSM);
            }


            if (teamID == 1) // if its team1's flag
            {
                if (collision.gameObject.CompareTag("p1") || collision.gameObject.CompareTag("p3"))
                {
                    GetFlag(collision.gameObject);
                }
            }
            else if (teamID == 2) // if its team2's flag
            {
                if (collision.gameObject.CompareTag("p2") || collision.gameObject.CompareTag("p4"))
                {
                    GetFlag(collision.gameObject);
                }
            }
        }
    }

    private void OnGetHit(CombatStateManager combat)
    {
        Debug.Log(combat.gameObject.tag + " took damage");



        if (flagWithPlayer && combat.Equals(curCSM))
        {
            curCSM.IdleState.hasChannelKey = false;
            sr.enabled = true;
            gameObject.transform.position = curCSM.transform.position;
            flagWithPlayer = false;
            StartCoroutine(FlagIsInvulnerable());
        }
    }

    private IEnumerator FlagIsInvulnerable()
    {
        yield return new WaitForSeconds(2); // wait for 2 seconds
        flagCollider.enabled = true;
    }

    private void GetFlag(GameObject gm)
    {
        Debug.Log(gm.tag + " captured the flag");
        curCSM.IdleState.hasChannelKey = true;
        sr.enabled = false;
        flagCollider.enabled = false;
        flagWithPlayer = true;
    }
}
