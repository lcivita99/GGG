using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerStateManager : MonoBehaviour
{

    public CrawlerBaseState currentState;

    public CrawlingState CrawlingState = new CrawlingState();
    public CrawlerHitstunState HitstunState = new CrawlerHitstunState();
    public CrawlerKnockbackState KnockbackState = new CrawlerKnockbackState();
    public GetUpState GetUpState = new GetUpState();

    public string currentStateString;


    private bool started;

    [SerializeField] public Transform initialPosition;
    [SerializeField] public Transform firstCorner;
    [SerializeField] public Transform secondCorner;
    [SerializeField] public Transform finishLine;

    public float timer;

    public float timeToFirstCorner;
    public float timeToSecondCorner;
    public float timeToFinishLine;
    public float totalTime;

    public float speed;

    public float firstDistance;
    public float secondDistance;
    public float thirdDistance;
    public float totalDistance;

    public bool goingBackwards;

    public float speedBuff;

    


    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        started = true;
        currentState = CrawlingState;
        currentState.EnterState(this);
        timer = 0f;
        totalTime = 15f;

        HitstunState.hitstunLength = 0.5f;

        goingBackwards = false;

        firstDistance = Vector2.Distance(initialPosition.position, firstCorner.position);
        secondDistance = Vector2.Distance(firstCorner.position, secondCorner.position);
        thirdDistance = Vector2.Distance(secondCorner.position, finishLine.position);
        totalDistance = firstDistance + secondDistance + thirdDistance;

        speed = totalDistance / totalTime;

        timeToFirstCorner = firstDistance / speed;
        timeToSecondCorner = secondDistance / speed;
        timeToFinishLine = thirdDistance / speed;

        GetUpState.getUpLength = 0.5f;
        KnockbackState.knockbackSpeed = 5f;

        speedBuff = 1f;




    }
    // Start is called before the first frame update
    void Update()
    {
        if (!started)
        {
            return;
        }

        currentState.UpdateState(this);

        currentStateString = currentState.ToString();


        if (timer <= timeToFirstCorner)
        {
            Crawl(initialPosition, firstCorner, 0f, timeToFirstCorner);
            //transform.position = Vector2.Lerp(initialPosition.position, firstCorner.position, timer / timeToFirstCorner);
        }

        else if (timer <= timeToFirstCorner + timeToSecondCorner)
        {
            Crawl(firstCorner, secondCorner, timeToFirstCorner, timeToSecondCorner);
            //transform.position = Vector2.Lerp(firstCorner.position, secondCorner.position, (timer - timeToFirstCorner) / timeToSecondCorner);
        }
        else if (timer <= timeToFirstCorner + timeToSecondCorner + timeToFinishLine)
        {
            Crawl(secondCorner, finishLine, timeToFirstCorner + timeToSecondCorner, timeToFinishLine);
        }

    }

    private void Crawl(Transform initialPosition, Transform finalPosition, float timeElapsed, float timeToend)
    {
        //transform.position = Vector2.Lerp(circle2.position, circle3.position, (timer - timeToCircle2) / timeToCircle3);

        // speed = distance / time


        transform.position = Vector2.Lerp(initialPosition.position, finalPosition.position, (timer - timeElapsed) / timeToend);
        //transform.position = Vector2.Lerp(initialPosition.position, finalPosition.position, (timer - timeElapsed) / timeToend);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        currentState.OnTriggerStay(this, collision);
        //Debug.Log("thinks is in trigger");

        //UpdateGettingHitTimers(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //ResetGettingHitTimers(collision);
    }

    public void SwitchState(CrawlerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void SwitchState(CrawlerBaseState state, float number)
    {
        currentState = state;
        currentState.EnterState(this, number);
    }

    public void SwitchState(CrawlerBaseState state, float number, string str)
    {
        currentState = state;
        currentState.EnterState(this, number, str);
    }

}
