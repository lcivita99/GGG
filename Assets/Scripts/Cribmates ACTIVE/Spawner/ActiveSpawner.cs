using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSpawner : PlaceableObj
{
    [SerializeField] private GameObject enemy;

    [HideInInspector] public int enemiesInPool;

    [HideInInspector] public Vector2 closestEnemyPosition;
    [HideInInspector] public CombatStateManager closestEnemyCSM;

    [HideInInspector] private float spawnTimer;

    public Transform target;

    //[HideInInspector] private float idleRotateSpeed;

    public float spawnRate;

    public bool enemyDisabled;


    // has this format so we can call the "base.Start()" function
    // which is the Start of the parent class
    protected override void Start()
    {
        base.Start();
        maxHealth = 10;
        curHealth = maxHealth;
        spawnRate = 2f;
        enemyDisabled = true;
    }

    void Update()
    {
        SetClosestEnemyPosition();
        Spawning();
        
    }

    private void SetClosestEnemyPosition()
    {
        closestEnemyPosition = enemyObjs[0].transform.position;
        target = enemyObjs[0].transform;
        closestEnemyCSM = enemyCSMs[0];

        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (Vector2.Distance(transform.position, enemyObjs[i].transform.position) < Vector2.Distance(transform.position, closestEnemyPosition))
            {
                closestEnemyPosition = enemyObjs[i].transform.position;
                closestEnemyCSM = enemyCSMs[i];
                target = enemyObjs[i].transform;
            }
        }
    }

    private void Spawning()
    {
        if (enemyDisabled && !closestEnemyCSM.untargettable)
        spawnTimer += Time.deltaTime;


        //TODO: not sure if this is needed, it might always just spawn in same direction
        //TODO: !!!!!!!!!!!!!!!(what if you block all directions????)!!!!!!!
        //transform.up = (closestEnemyPosition - new Vector2(transform.position.x, transform.position.y)).normalized;

        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            Spawn();
        }
    }

    

    private void Spawn()
    {
        enemyDisabled = false;
        enemy.SetActive(true);
    }
}
