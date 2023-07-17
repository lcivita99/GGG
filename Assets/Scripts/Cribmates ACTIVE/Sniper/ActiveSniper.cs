using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSniper : PlaceableObj
{
    //[SerializeField] private GameObject bullet;
    [SerializeField] private GameObject sniper;
    [SerializeField] private GameObject sniperShot;

    [HideInInspector] public Vector2 closestEnemyPosition;

    [HideInInspector] private float shotTimer;
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


    void Start()
    {
        //myTeam = 1;
        //fireRate = 8f;
        line = GetComponentInChildren<LineRenderer>();
        line.SetPosition(0, sniper.transform.localPosition);
        activatedShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        shotTimer += Time.deltaTime;

        sniper.transform.up = (closestEnemyPosition - new Vector2(sniper.transform.position.x, sniper.transform.position.y)).normalized;

        if (shotTimer <= timeBetweenShots)
        {
            LineClear();
            SetClosestEnemyPosition();
            UpdateLineWidth(lineWidthCurve1, 0, timeBetweenShots);
        }
        
        else if (shotTimer <= timeBetweenShots + timeAiming)
        {
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

        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (Vector2.Distance(transform.position, enemyObjs[i].transform.position) < Vector2.Distance(transform.position, closestEnemyPosition))
            {
                closestEnemyPosition = enemyObjs[i].transform.position;
            }
        }
    }

    private void UpdateShotTarget(bool preShot)
    {
        // This creates a LayerMask with all bits set to 1, meaning all layers are included.
        int layerMask = ~0;

        // Then we turn off bit 16 (for layer 16) by using bitwise negation (~).
        // This sets the bit for layer 16 to 0, excluding it from the LayerMask.
        layerMask = layerMask & ~(1 << 16);

        RaycastHit2D hit = Physics2D.Raycast(sniper.transform.position, sniper.transform.up, Mathf.Infinity, layerMask);
        if (hit.collider != null)
        {
            Vector2 linePos2;
            if (preShot)
            {
                linePos2 = closestEnemyPosition - new Vector2(transform.position.x, transform.position.y);
            } else
            {
                linePos2 = (hit.point - new Vector2(transform.position.x, transform.position.y)).normalized * 50;
            }
            line.SetPosition(1, linePos2);
            //Debug.Log(hit.transform.position);
        }

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
