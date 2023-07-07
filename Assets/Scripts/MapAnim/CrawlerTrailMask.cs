using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerTrailMask : MonoBehaviour
{
    private bool hasStarted;

    [SerializeField] private Transform spriteMask;
    [SerializeField] private Transform transformRef1;
    [SerializeField] private Transform transformRef2;
    [SerializeField] private Transform transformRef3;
    [SerializeField] private Transform transformRef4;

    [SerializeField] private CrawlerStateManager crawlerManager;


    private float totalTime;
    private float timeToRef2;
    private float timeToRef3;
    private float timeToRef4;

    private float firstDistance;
    private float secondDistance;
    private float thirdDistance;
    private float totalDistance;

    private float speed;

    [SerializeField] private string crawlerID;

    private float offsetDir;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.1f);
        hasStarted = true;
        totalTime = crawlerManager.totalTime;

        firstDistance = Vector2.Distance(transformRef1.position, transformRef2.position);
        secondDistance = Vector2.Distance(transformRef2.position, transformRef3.position);
        thirdDistance = Vector2.Distance(transformRef3.position, transformRef4.position);
        totalDistance = firstDistance + secondDistance + thirdDistance;

        speed = totalDistance / totalTime;

        timeToRef2 = firstDistance / speed;
        timeToRef3 = secondDistance / speed;
        timeToRef4 = thirdDistance / speed;

        if (crawlerID == "right")
        {
            offsetDir = 1;
        }
        else
        {
            offsetDir = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            return;
        }

        if (crawlerManager.timer <= timeToRef2)
        {
            Crawl(transformRef1, transformRef2, 0f, timeToRef2, Quaternion.identity);
            //transform.position = Vector2.Lerp(initialPosition.position, firstCorner.position, timer / timeToFirstCorner);
        }

        else if (crawlerManager.timer <= timeToRef2 + timeToRef3)
        {
            Crawl(transformRef2, transformRef3, timeToRef2, timeToRef3, Quaternion.identity);
            //transform.position = Vector2.Lerp(firstCorner.position, secondCorner.position, (timer - timeToFirstCorner) / timeToSecondCorner);
        }
        else if (crawlerManager.timer <= timeToRef2 + timeToRef3 + timeToRef4)
        {
            Crawl(transformRef3, transformRef4, timeToRef2 + timeToRef3, timeToRef4, transformRef3.rotation);
        }
    }

    private void Crawl(Transform initialPosition, Transform finalPosition, float timeElapsed, float timeToend, Quaternion targetRot)
    {
        spriteMask.position = Vector2.Lerp(initialPosition.position, finalPosition.position, (crawlerManager.timer - timeElapsed) / timeToend);
        spriteMask.rotation = targetRot;

        if (targetRot == transformRef3.rotation)
        {
            spriteMask.position += transformRef3.right * 20 * offsetDir;
        }
        //spriteMask.rotation = Quaternion.Lerp(initialPosition.rotation, finalPosition.rotation, (crawlerManager.timer - timeElapsed) / timeToend);
    }
}
