using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CribmateManager : MonoBehaviour
{

    public CribmateStats stats;
    private ShopManager shop = ShopManager.instance;

    private void Awake()
    {
      
    }

    public void SetStats(CribmateStats statsSetter)
    {
        stats = (CribmateStats)statsSetter.Clone();
    }
}
