using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] public Dictionary<GameObject, (Vector2, bool)> interactableObjs = new Dictionary<GameObject, (Vector2, bool)>();
    public bool canInteract;
    public List<PlayerInteractionManager> playerInteractionManagers = new List<PlayerInteractionManager>();

    void Start()
    {
        
    }

    // to be called when creating an interactable obj (should be called in each player's scripts
    public void AddInteractableObj(GameObject obj)
    {
        bool interactable;
        Vector2 range;

        range = obj.GetComponent<InteractableObject>().range;

        if (Mathf.Abs(transform.position.x - obj.transform.position.x) < range.x &&
            Mathf.Abs(transform.position.y - obj.transform.position.y) < range.y)
        {
            interactable = true;
        }
        else
        {
           // Debug.Log(true);
            interactable = false;
        }

        interactableObjs.Add(obj, (range, interactable));
    }

    public void RemoveInteractableObj(GameObject obj)
    {
        interactableObjs.Remove(obj);
    }

    // ! Late update so that adding or removing would be considered that frame
    void LateUpdate()
    {
        UpdateInteractableDictionary();
    }

    private void IsCloseEnough(GameObject obj, float xRange, float yRange)
    {
        bool isCloseEnough = Mathf.Abs(transform.position.x - obj.transform.position.x) < xRange &&
                             Mathf.Abs(transform.position.y - obj.transform.position.y) < yRange;

        var tempTuple = interactableObjs[obj];
        interactableObjs[obj] = (tempTuple.Item1, isCloseEnough);
    }

    private void UpdateInteractableDictionary()
    {
        var keys = new List<GameObject>(interactableObjs.Keys);

        foreach (GameObject key in keys)
        {
            var tuple = interactableObjs[key];

            IsCloseEnough(key, tuple.Item1.x, tuple.Item1.y);
        }
    }
}


// each interactable obj has an InteractableObject script with a vector 2. This script is what controls the shader. It checks each PlayerInteractionManager's dictionary to see if it has the bool true associated with it.

// 4 MAIN DICTIONARies --> (OBJS, (Vector2, bool))

// INSIDE EACH PLAYER:
     // GO THROUGH ARRAY CHECKING ISCLOSEENOUGH
     // IF IT IS:
        // THE bool = true;
        // THE PLAYER CAN PRESS "Cross" (idle/respawn state)
    // IF ITS NOT:
        // The bool = false;


// each object checks the 4 dictionaries for itself and decides to glow or not. 
        
