using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingVars : MonoBehaviour
{
    public static PlacingVars instance { get; set; }

    public List<GameObject> holograms;
    public List<GameObject> prefabs;

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
