using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFarm : PlaceableObj
{
    [HideInInspector] private float farmTimer;
    public float farmRate;




    void Start()
    {

        maxHealth = 15;
        curHealth = maxHealth;
        farmRate = 5f;

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
