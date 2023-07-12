using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int currency = 0;
    public float passiveIncomePerMinute = 1;
    public float passiveIncomeTimer = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PassiveIncome();
    }

    public void PassiveIncome()
    {
        passiveIncomeTimer += Time.deltaTime;

        float timeToAddCoin = 60 / passiveIncomePerMinute;

        if (passiveIncomeTimer >= timeToAddCoin)
        {
            currency++;
            passiveIncomeTimer = 0;
        }
    }
}
