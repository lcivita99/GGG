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

    public GameObject healthbar;

    // has this format so we can call the "base.Start()" function from the children classes
    // which is this Start. It does not run otherwise
    protected virtual void Start()
    {
        healthbar = Instantiate(PlaceableHealthbar.instance.healthbarPrefab, transform.position + Vector3.up * 0.8f + Vector3.left * 0.625f, Quaternion.identity);
        healthbar.transform.SetParent(transform);
        originalPosition = transform.position;
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
            Shake();
            UpdateHealthUI();
        } else if (collision.gameObject.layer.Equals(7))
        {
            curHealth -= 2;
            Shake();
            UpdateHealthUI();
            //Debug.Log(curHealth);
        }

        if (curHealth <= 0)
        {
            Bounds bounds = GetComponent<Collider2D>().bounds;
            var guo = new GraphUpdateObject(bounds);
            GridManager.instance.RemoveFromGrid(transform.position);

            Destroy(gameObject);
            Destroy(healthbar);
            // update pathfinding

            // Set some settings
            guo.updatePhysics = true;
            //guo.updatePhysics = false;
            AstarPath.active.UpdateGraphs(guo);
            //free from grid

           
        }
    }

    private void UpdateHealthUI()
    {
        float cur = curHealth;
        float max = maxHealth;
        if (cur < 0)
        {
            cur = 0;
        }

        float healthRatio = cur / max;

        healthbar.transform.localScale = new Vector3(0.4f * healthRatio, 1, 1);

    }

    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.1f;

    private Vector3 originalPosition;


    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        // Save the starting time of the shake
        float startTime = Time.time;

        while (Time.time < startTime + shakeDuration)
        {
            // Generate random offsets for the shake effect
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

            // Apply the shake effect to the object's position
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0f);

            yield return null; // Wait for the next frame
        }

        // Set the transform back to its original position after the shake
        transform.position = originalPosition;
    }
}
