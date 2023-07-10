using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CribmateManager : MonoBehaviour
{
    public CribmateStats stats;
    private ShopManager shop = ShopManager.instance;

    public Material originalMaterial; // Reference to the original material
    private Material instanceMaterial; // Instance material for this specific object

    public LayerMask playerTargetLayer;
    public LayerMask invulnerableTargetLayer;

    private void Start()
    {
        // Create an instance of the original material
        instanceMaterial = Instantiate(originalMaterial);

        // Assign the instance material to the object
        GetComponent<Renderer>().material = instanceMaterial;

        playerTargetLayer = LayerMask.NameToLayer("player");
        playerTargetLayer = LayerMask.NameToLayer("invulnerable");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    public void SetStats(CribmateStats statsSetter)
    {
        stats = (CribmateStats)statsSetter.Clone();
    }

    // outline stuff
    public void EnableOutline()
    {
        instanceMaterial.SetFloat("_EnableOutline", 1.0f);
    }

    public void DisableOutline()
    {
        instanceMaterial.SetFloat("_EnableOutline", 0f);
    }

    public void SetOutlineColor(Color color)
    {
        instanceMaterial.SetColor("_OutlineColor", color);
    }
}
