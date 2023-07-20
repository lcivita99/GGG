using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTurret : PlaceableObj
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private List<GameObject> bullets = new List<GameObject>();

    [HideInInspector] public float turretAttackRadius;

    [SerializeField] public GameObject turret;

    [HideInInspector] public int bulletsInPool;

    [HideInInspector] public Vector2 closestEnemyPosition;
    [HideInInspector] public CombatStateManager closestEnemyCSM;

    [HideInInspector] private float shotTimer;
    [HideInInspector] private float idleTimer;

    //[HideInInspector] private float idleRotateSpeed;

    private List<Vector2> directions = new List<Vector2>();
    private int idleIdx = 0;

    public float fireRate;

    public bool IsCloseEnough()
    {
        if (Vector2.Distance(closestEnemyPosition, turret.transform.position) <= turretAttackRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // has this format so we can call the "base.Start()" function
    // which is the Start of the parent class
    protected override void Start()
    {
        base.Start();
        maxHealth = 10;
        curHealth = maxHealth;
        fireRate = 1f;
        turretAttackRadius = 4f;
        //idleRotateSpeed = 50f;

        directions.Add(Vector2.up);
        directions.Add(Vector2.right);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);

        //healthbar = Instantiate(PlaceableHealthbar.instance.healthbarPrefab, transform.position + Vector3.up + Vector3.left * 0.625f, Quaternion.identity);
    }

    void Update()
    {
        SetClosestEnemyPosition();

        if (IsCloseEnough() && !closestEnemyCSM.untargettable)
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
        closestEnemyCSM = enemyCSMs[0];

        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (Vector2.Distance(transform.position, enemyObjs[i].transform.position) < Vector2.Distance(turret.transform.position, closestEnemyPosition) 
                && !enemyCSMs[i].untargettable)
            {
                closestEnemyPosition = enemyObjs[i].transform.position;
                closestEnemyCSM = enemyCSMs[i];

            }
        }
    }

    private void Shooting()
    {
        shotTimer += Time.deltaTime;

        turret.transform.up = (closestEnemyPosition - new Vector2(turret.transform.position.x, turret.transform.position.y)).normalized;

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

            turret.transform.up = directions[idleIdx];
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
