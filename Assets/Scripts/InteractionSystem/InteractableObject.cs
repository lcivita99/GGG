using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public List<PlayerInteractionManager> playerInteractionManagers = new List<PlayerInteractionManager>();

    public Material originalMaterial; // Reference to the original material
    public Material instanceMaterial; // Instance material for this specific object
    public float timeToChannel;

    

    //public LayerMask playerTargetLayer;
    //public LayerMask invulnerableTargetLayer;

    private bool outlineEnabled;

    public Vector2 range;
    void Start()
    {
        //if (int = 0 )
        //{
        //finishChannelling = new TemporaryInterface();

        //} else if (int = 1)
        //{

        //}
        // Create an instance of the original material

            instanceMaterial = Instantiate(originalMaterial);

            // Assign the instance material to the object
            GetComponent<Renderer>().material = instanceMaterial;

        

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

        DisableOutline();
    }

    void Update()
    {
        CheckIfInteractable();
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

    public void CheckIfInteractable()
    {
        bool interactable = false;
        for (int i = 0; i < playerInteractionManagers.Count; i++)
        {
            
            if (playerInteractionManagers[i].interactableObjs[gameObject].Item2)
            {
                interactable = true;
                break;
            }
        }
        if (interactable)
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

    public virtual void FinishChannelling(CombatStateManager combat, bool idleState)
    {

        Debug.Log("CHANNEL FINISHED");
    }

    //private void CheckPlayerDistance()
    //{
    //    bool playerFound = false;
    //    for (int i = 0; i < players.Count; i++)
    //    {
    //        if (Mathf.Abs(transform.position.x - players[i].position.x) < xRange &&
    //            Mathf.Abs(transform.position.y - players[i].position.y) < yRange)
    //        {
    //            playerFound = true;
    //            //players[i].GetComponent<CombatStateManager>().
    //        }
    //    }
    //    if (playerFound)
    //    {
    //        if (!outlineEnabled)
    //        {
    //            EnableOutline();
    //        }
    //    }
    //    else
    //    {
    //        if (outlineEnabled)
    //        {
    //            DisableOutline();
    //        }
    //    }
    //}
}
