using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CribmateManager : MonoBehaviour
{
    public CribmateStats stats;
    private ShopManager shop = ShopManager.instance;

    public Material originalMaterial; // Reference to the original material
    private Material instanceMaterial; // Instance material for this specific object

    public LayerMask playerTargetLayer;
    public LayerMask invulnerableTargetLayer;

    private bool outlineEnabled;
    private float xRange = 2;
    private float yRange = 4;

    private List<Transform> players = new List<Transform>();



    private void Start()
    {
        // Create an instance of the original material
        instanceMaterial = Instantiate(originalMaterial);

        // Assign the instance material to the object
        GetComponent<Renderer>().material = instanceMaterial;

        if (GameObject.FindGameObjectWithTag("p1") != null)
        {
            players.Add(GameObject.FindGameObjectWithTag("p1").transform);
        }
        if (GameObject.FindGameObjectWithTag("p2") != null)
        {
            players.Add(GameObject.FindGameObjectWithTag("p2").transform);
        }
        if (GameObject.FindGameObjectWithTag("p3") != null)
        {
            players.Add(GameObject.FindGameObjectWithTag("p3").transform);
        }
        if (GameObject.FindGameObjectWithTag("p4") != null)
        {
            players.Add(GameObject.FindGameObjectWithTag("p4").transform);
        }

        DisableOutline();
    }

    private void Update()
    {
        CheckPlayerDistance();
    }

    public void SetStats(CribmateStats statsSetter)
    {
        stats = (CribmateStats)statsSetter.Clone();
    }

    // outline stuff
    public void EnableOutline()
    {
        outlineEnabled = true;
        instanceMaterial.SetFloat("_EnableOutline", 1.0f);
    }

    public void DisableOutline()
    {
        outlineEnabled = false;
        instanceMaterial.SetFloat("_EnableOutline", 0f);
    }

    public void SetOutlineColor(Color color)
    {
        instanceMaterial.SetColor("_OutlineColor", color);
    }

    private void CheckPlayerDistance()
    {
        bool playerFound = false;
        for (int i = 0; i < players.Count; i++)
        {
            if (Mathf.Abs(transform.position.x - players[i].position.x) < xRange &&
                Mathf.Abs(transform.position.y - players[i].position.y) < yRange)
            {
                playerFound = true;
                break;
            }
        }
        if (playerFound)
        {
            if (!outlineEnabled)
            {
                EnableOutline();
            }
        } else
        {
            if (outlineEnabled)
            {
                DisableOutline();
            }
        }
    }
}
