using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSniper : PlaceableObj
{
    //[SerializeField] private GameObject bullet;
    [SerializeField] private GameObject sniper;
    [SerializeField] private GameObject sniperShot;

    [HideInInspector] public Vector2 closestEnemyPosition;
    [HideInInspector] public Vector2 shotTarget;

    [HideInInspector] public CombatStateManager closestEnemyCSM;

    [HideInInspector] private float shotTimer;
    private float prevShotTimer;
    private bool noPlayersTargettable;
    private float timeBetweenShots = 4;
    private float timeAiming = 2;
    private float timeChanelling = 0.5f;
    private float timeShooting = 0.5f;

    public float fireRate;

    public LineRenderer line;

    [SerializeField] private AnimationCurve lineWidthCurve1;
    [SerializeField] private AnimationCurve lineWidthCurve2;
    [SerializeField] private AnimationCurve lineWidthCurve3;
    [SerializeField] private AnimationCurve lineWidthCurve4;

    private bool activatedShot;

    // has this format so we can call the "base.Start()" function
    // which is the Start of the parent class
    protected override void Start()
    {
        base.Start();
        //myTeam = 1;
        //fireRate = 8f;
        SetClosestEnemyPosition();
        maxHealth = 10;
        curHealth = maxHealth;
        line = GetComponentInChildren<LineRenderer>();
        line.SetPosition(0, sniper.transform.localPosition);
        activatedShot = false;
        //healthbar = Instantiate(PlaceableHealthbar.instance.healthbarPrefab, transform.position + Vector3.up + Vector3.left * 0.625f, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!noPlayersTargettable)
        {
            shotTimer += Time.deltaTime;
        }
       

        sniper.transform.up = (closestEnemyPosition - new Vector2(sniper.transform.position.x, sniper.transform.position.y)).normalized;

        if (shotTimer <= timeBetweenShots)
        {
            if (closestEnemyCSM.untargettable)
            {
                noPlayersTargettable = true;
            }
            LineClear();
            //Debug.Log("Running");
            SetClosestEnemyPosition();
            UpdateLineWidth(lineWidthCurve1, 0, timeBetweenShots);
        }
        
        else if (shotTimer <= timeBetweenShots + timeAiming)
        {
            if (closestEnemyCSM.untargettable)
            {
                //Debug.Log("untargettable");

                noPlayersTargettable = true;
                shotTimer = timeBetweenShots;
            }
            LineRed();
            SetClosestEnemyPosition();
            UpdateShotTarget(true);
            UpdateLineWidth(lineWidthCurve2, timeBetweenShots, timeAiming);
            // Red laser aiming
        }

        else if (shotTimer <= timeBetweenShots + timeAiming + timeChanelling)
        {
            LineSemiTransparent();
            UpdateShotTarget(false);
            // stop rotation
            // line white not full
            UpdateLineWidth(lineWidthCurve3, timeBetweenShots + timeAiming, timeChanelling);
        }

        else if (shotTimer <= timeBetweenShots + timeAiming + timeChanelling + timeShooting)
        {
            if (!activatedShot)
            {
                activatedShot = true;
                sniperShot.SetActive(true);

            }
            LineWhite();
            UpdateShotTarget(false);
            // active shot
            UpdateLineWidth(lineWidthCurve4, timeBetweenShots + timeAiming + timeChanelling, timeShooting);
        }

        else
        {
            LineClear();
            
            shotTimer = 0;
            sniperShot.SetActive(false);
            activatedShot = false;
        }
    }

    private void SetClosestEnemyPosition()
    {
        closestEnemyPosition = enemyObjs[0].transform.position;
        closestEnemyCSM = enemyCSMs[0];
        if (!enemyCSMs[0].untargettable)
            noPlayersTargettable = false;

        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (Vector2.Distance(transform.position, enemyObjs[i].transform.position) < Vector2.Distance(transform.position, closestEnemyPosition)
                && !enemyCSMs[i].untargettable)
            {
                closestEnemyPosition = enemyObjs[i].transform.position;
                noPlayersTargettable = false;
                closestEnemyCSM = enemyCSMs[i];

            }
        }
    }

    private void UpdateShotTarget(bool preShot)
    { 

        Vector2 linePos2;
        if (preShot)
        {
            shotTarget = closestEnemyPosition;
            linePos2 = shotTarget - new Vector2(transform.position.x, transform.position.y);
        } else
        {
            linePos2 = (shotTarget - new Vector2(transform.position.x, transform.position.y)).normalized * 50;
        }
        line.SetPosition(1, linePos2);

    }

    private void LineClear()
    {
        line.startColor = Color.clear;
        line.endColor = Color.clear;
    }

    private void LineRed()
    {
        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    private void LineSemiTransparent()
    {
        line.startColor = new Color(1, 1, 1, 0.5f);
        line.endColor = new Color(1, 1, 1, 0.5f);
    }

    private void LineWhite()
    {
        line.startColor = Color.white;
        line.endColor = Color.white;
    }

    private void UpdateLineWidth(AnimationCurve curve, float timeElapsed, float segmentLength)
    {
        float width = curve.Evaluate((shotTimer - timeElapsed) / segmentLength);
        line.startWidth = width;
        line.endWidth = width;
    }
}
