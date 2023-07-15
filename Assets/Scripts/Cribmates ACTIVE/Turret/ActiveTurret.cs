using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTurret : PlaceableObj
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private List<GameObject> bullets = new List<GameObject>();

    [HideInInspector] public float turretAttackRadius;

    [HideInInspector] public int bulletsInPool;

    [HideInInspector] public Vector2 closestEnemyPosition;

    [HideInInspector] private float shotTimer;
    [HideInInspector] private float idleTimer;

    //[HideInInspector] private float idleRotateSpeed;

    private List<Vector2> directions = new List<Vector2>();
    private int idleIdx = 0;

    public float fireRate;

    public bool isCloseEnough()
    {
        if (Vector2.Distance(closestEnemyPosition, transform.position) <= turretAttackRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Start()
    {
        fireRate = 1f;
        turretAttackRadius = 4f;
        //idleRotateSpeed = 50f;

        directions.Add(Vector2.up);
        directions.Add(Vector2.right);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
    }

    void Update()
    {
        SetClosestEnemyPosition();

        if (isCloseEnough())
        {
            Shooting();
        }
        else
        {
            Idle();
        }
    }

    private void SetClosestEnemyPosition()
    {
        closestEnemyPosition = enemyObjs[0].transform.position;

        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (Vector2.Distance(transform.position, enemyObjs[i].transform.position) < Vector2.Distance(transform.position, closestEnemyPosition))
            {
                closestEnemyPosition = enemyObjs[i].transform.position;
            }
        }
    }

    private void Shooting()
    {
        shotTimer += Time.deltaTime;

        transform.up = (closestEnemyPosition - new Vector2(transform.position.x, transform.position.y)).normalized;

        if (shotTimer >= fireRate)
        {
            Shoot();
            shotTimer = 0;
        }
    }

    private void Idle()
    {
        shotTimer = 0;
        idleTimer += Time.deltaTime;

        if (idleTimer >= 0.6f)
        {
            if (idleIdx < directions.Count - 1)
            {
                idleIdx++;
            } else
            {
                idleIdx = 0;
            }
            
            transform.up = directions[idleIdx];
            idleTimer = 0;
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeSelf)
            {
                bullets[i].SetActive(true);
                break;
            }
        } 
    }
}
