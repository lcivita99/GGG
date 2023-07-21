using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFarm : PlaceableObj
{
    [HideInInspector] private float farmTimer;
    public float farmRate;

    private Animator anim;

    [SerializeField] private Transform coinProgress;

    private Vector3 coinProgressMaxScale;
    private float coinProgressY;
    private Vector3 coinProgressStartScale;

    // has this format so we can call the "base.Start()" function
    // which is the Start of the parent class
    protected override void Start()
    {
        
        base.Start();

        anim = GetComponent<Animator>();
        maxHealth = 15;
        curHealth = maxHealth;
        farmRate = 5f;

        coinProgressY = coinProgress.localScale.y;
        coinProgressMaxScale = new Vector3(coinProgress.localScale.x, coinProgressY, 1);
        coinProgressStartScale = new Vector3(0, coinProgressY, 1);

        coinProgress.localScale = coinProgressStartScale;

        //healthbar = Instantiate(PlaceableHealthbar.instance.healthbarPrefab, transform.position + Vector3.up + Vector3.left * 0.625f, Quaternion.identity);

    }

    void Update()
    {
        Farming();

        coinProgress.localScale = Vector3.Lerp(coinProgressStartScale, coinProgressMaxScale, farmTimer / farmRate);

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
       
        placer.currencyManager.ChangeCurrency(1);
        anim.SetTrigger("collect");
    }
}
