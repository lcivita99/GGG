using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFarm : PlaceableObj
{
    [HideInInspector] private float farmTimer;
    public float farmRate;



    // has this format so we can call the "base.Start()" function
    // which is the Start of the parent class
    protected override void Start()
    {
        
        base.Start();
        maxHealth = 15;
        curHealth = maxHealth;
        farmRate = 5f;

        //healthbar = Instantiate(PlaceableHealthbar.instance.healthbarPrefab, transform.position + Vector3.up + Vector3.left * 0.625f, Quaternion.identity);

    }

    void Update()
    {
        Farming();

    }


    private void Farming()
    {

        farmTimer += Time.deltaTime;



        if (farmTimer >= farmRate)
        {
            farmTimer = 0;
            FarmCoin();
        }
    }



    private void FarmCoin()
    {
        placer.currencyManager.currency++;
    }
}
