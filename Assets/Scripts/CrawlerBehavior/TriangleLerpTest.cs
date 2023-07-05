using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleLerpTest : MonoBehaviour
{
    [SerializeField] private Transform circle1;
    [SerializeField] private Transform circle2;
    [SerializeField] private Transform circle3;

    private float timer;

    private float timeToCircle2 = 5;
    private float timeToCircle3 = 5;

    private bool isGoingBackwards;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGoingBackwards)
        {
            timer += Time.deltaTime;
        } else
        {
            timer -= 2 * Time.deltaTime;
        }

        if (timer <= timeToCircle2)
        {
            transform.position = Vector2.Lerp(circle1.position, circle2.position, timer / timeToCircle2);
        }

        else if (timer <= timeToCircle2 + timeToCircle3)
        {
            transform.position = Vector2.Lerp(circle2.position, circle3.position, (timer - timeToCircle2) / timeToCircle3);
        }
        
        if (timer >= 3 * (timeToCircle2 + timeToCircle3) / 4)
        {
            isGoingBackwards = true;
        }
    }
}
