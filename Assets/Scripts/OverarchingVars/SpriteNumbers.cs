using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteNumbers : MonoBehaviour
{
    public static SpriteNumbers instance { get; set; }

    public Sprite[] numbers;
    void Awake()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
