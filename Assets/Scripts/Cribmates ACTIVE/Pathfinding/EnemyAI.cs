using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed;
    public float nextWaypointDistance;

    private Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    [SerializeField] private GameObject spriteObject;
    private Vector2 spriteUpTarget;

    void Start()
    {
        nextWaypointDistance = 1f;
        speed = 1000f;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        InvokeRepeating("SetSpriteTransform", 0.2f, 0.07f);
    }

    private void SetSpriteTransform()
    {
        spriteObject.transform.position = transform.position;
        spriteObject.transform.up = spriteUpTarget;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = speed * Time.deltaTime * dir;

        Debug.Log(dir);

        rb.AddForce(force);
        if (dir != Vector2.zero)
        {
            //spriteObject.transform.up = Vector3.MoveTowards(spriteObject.transform.up, force.normalized, Time.deltaTime * 4);
            //spriteObject.transform.up = force.normalized;
            spriteUpTarget = rb.velocity.normalized;

        }
        
        

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
