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
    //bool reachedEndOfPath = false;

    ActiveSpawner spawner;
    [SerializeField] private Transform spawnerPos;

    Seeker seeker;
    Rigidbody2D rb;

    [SerializeField] private GameObject spriteObject;
    private Vector2 spriteUpTarget;

    public float maxTime = 5f;
    public float timer;

    private float enemyShieldStunLength = 0.2f;

    CombatStateManager enemyCSM;



    private void OnEnable()
    {
       if (spawner != null)
        {
            target = spawner.target;
            enemyCSM = spawner.closestEnemyCSM;
        }
       
       timer = 0f;
       //transform.localPosition = Vector2.zero;
    }

    private void OnDisable()
    {
        spawner.enemyDisabled = true;
        rb.velocity = Vector2.zero;
        transform.localPosition = Vector2.zero;
        // TODO instantiate bullet death
    }

    void Awake()
    {
        timer = 0f;
        spawner = GetComponentInParent<ActiveSpawner>();
        target = spawner.target;
        //Debug.Log(spawner.closestEnemyCSM);
        enemyCSM = spawner.closestEnemyCSM;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(3)) // player
        {
            if (IsEnemyPlayer(collision)) // what kind of player? 
            {
                Debug.Log("Collision");
                // get combat state manager:
                CombatStateManager combat = collision.GetComponent<CombatStateManager>();
                // calculate direction: 

                //

                // calculate vector 2: 
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                if (combat.currentState == combat.ShieldState || combat.currentState == combat.ShieldStunState)
                {
                    combat.SwitchState(combat.ShieldStunState, enemyShieldStunLength, "", dir);
                }
                else
                {
                    combat.currentState.ForcedOutOfState(combat);
                    //T 
                    combat.SwitchState(combat.HitstunState, 0, "bullet", dir);
                }
                transform.parent.gameObject.SetActive(false);
            }
        }
        //else if (collision.gameObject.layer.Equals(10)) // wall
        //{
        //    transform.parent.gameObject.SetActive(false);
        //}
    }

    private bool IsEnemyPlayer(Collider2D collision)
    {
        int spawnerTeam = spawner.gameObject.GetComponent<PlaceableObj>().myTeam;
        int colliderTeam = collision.gameObject.GetComponent<PlayerMovement>().team;
        if (spawnerTeam == colliderTeam)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= maxTime)
        {
            // TODO: AOE Destroy
            transform.parent.gameObject.SetActive(false);
        }
        

        if (currentWaypoint >= path.vectorPath.Count)
        {
            //reachedEndOfPath = true;
            return;
        } else
        {
           // reachedEndOfPath = false;
        }

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = speed * Time.deltaTime * dir;

        
        if (!enemyCSM.untargettable)
        {
            rb.AddForce(force);
        }
       
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
