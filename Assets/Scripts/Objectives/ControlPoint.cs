using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    public List<Vector2> positions = new List<Vector2>();

   // public GameObject controlPoint;

    public List<string> playersInCP = new List<string>();

    public bool team1InCP;
    public bool team2InCP;

    public float team1Timer = 0f;
    public float team2Timer = 0f;


    public float timeToCapture = 10f;

    public SpriteRenderer cpSprite;


    private void Start()
    {
        cpSprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(3))
        {
            playersInCP.Add(collision.gameObject.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(3))
        {
            playersInCP.Remove(collision.gameObject.tag);
        }
    }

    private void Update()
    {
        team1InCP = false;
        team2InCP = false;

        foreach (string tag in playersInCP)
        {
            if (tag == "p1" || tag == "p3")
            {
                team1InCP = true;
            }
            else if (tag == "p2" || tag == "p4")
            {
                team2InCP = true;
            }
        }

        if (team1InCP && team2InCP)
        {
            //contested
            cpSprite.color = Color.yellow;
        }
        else if (team1InCP)
        {
            //team1 progress
            team1Timer += Time.deltaTime;
            cpSprite.color = Color.blue;
        }
        else if (team2InCP)
        {
            //team2 progress
            team2Timer += Time.deltaTime;
            
            cpSprite.color = new Color(team2Timer/timeToCapture, 0, 0);
        }
        else
        {
            //no progress
            cpSprite.color = Color.white;
        }

        if (team1Timer >= timeToCapture)
        {
            Debug.Log("team1 wins!");
        }
        else if (team2Timer >= timeToCapture)
        {
            Debug.Log("team2 wins!");
        }
    }
}
