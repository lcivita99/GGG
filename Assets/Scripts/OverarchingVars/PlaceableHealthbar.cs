using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableHealthbar : MonoBehaviour
{
    public static PlaceableHealthbar instance { get; set; }

    public GameObject healthbarPrefab;

    private void Awake()
    {
        // Singleton!
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
