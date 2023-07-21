using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMapManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectives;

    public static EventMapManager instance;
    public float timer;
    public float objectiveStartTime;
    public bool objectiveInProgress;

    private GameObject curObjective;

    // Declare the delegate (if using non-generic pattern).
    public delegate void MyEventHandler();

    // Declare the event using the delegate.
    public event MyEventHandler EndEvent;




    private void Start()
    {
        // Singleton!
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        objectiveInProgress = false;

        objectiveStartTime = 1f;
    }

    private void Update()
    {
        if (!objectiveInProgress)
        {
            timer += Time.deltaTime;
        }

        if (timer >= objectiveStartTime && objectives.Count != 0)
        {
            timer = 0;
            objectiveInProgress = true;
            curObjective = objectives[Random.Range(0, objectives.Count)];
            Instantiate(curObjective, Vector2.zero, Quaternion.identity);
        }
    }

    public void EndCurEvent()
    {
        // Trigger the event when called.
        EndEvent?.Invoke();


        objectiveInProgress = false;
        objectives.Remove(curObjective);
        //Destroy(curObjective);
        Debug.Log("obj ended");
    }

}
