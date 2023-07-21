using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringFlag : InteractableObject
{

    //public List<PlayerInteractionManager> playerInteractionManagers = new List<PlayerInteractionManager>();

    public GameObject flag;
    public int teamID;




    private void OnEnable()
    {
        // Subscribe to the event when this script becomes active/enabled.
        EventMapManager.instance.EndEvent += ObjectiveEnded;
    }

    private void ObjectiveEnded()
    {
        // Unsubscribe from EndEvent (prevent memory leak)
        EventMapManager.instance.EndEvent -= ObjectiveEnded;

        

        gameObject.SetActive(false);

    }


    private void Start()
    {

        instanceMaterial = Instantiate(originalMaterial);

        instanceMaterial.SetFloat("_EnableOutline", 0f);

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

        for (int i = 0; i < playerInteractionManagers.Count; i++)
        {
            playerInteractionManagers[i].AddInteractableObj(gameObject);
        }
        // Create an instance of the original material


        //// add to dictionary
        //for (int i = 0; i < playerInteractionManagers.Count; i++)
        //{
        //    Debug.Log("I've been added");
        //    playerInteractionManagers[i].AddInteractableObj(gameObject);
        //}
    }


    //when the objective ends:
    public void RemoveFromInteraction()
    {
        for (int i = 0; i < playerInteractionManagers.Count; i++)
        {
            playerInteractionManagers[i].RemoveInteractableObj(gameObject);
        }
    }



    public override void FinishChannelling(CombatStateManager combat, bool idleState)
    {
        transform.parent.gameObject.SetActive(false);
        // who won

        EventMapManager.instance.EndCurEvent();
    }



}
