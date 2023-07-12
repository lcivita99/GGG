using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int currency = 0;
    public float passiveIncomePerMinute = 1;
    public float passiveIncomeTimer = 0;

    public SpriteRenderer currencyUI;


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
            ChangeCurrency(1);
            passiveIncomeTimer = 0;
        }
    }

    public void ChangeCurrency(int amount)
    {
        currency += amount;

        currency = Mathf.Clamp(currency, 0, 99);

        currencyUI.sprite = SpriteNumbers.instance.numbers[currency];
    }
}
