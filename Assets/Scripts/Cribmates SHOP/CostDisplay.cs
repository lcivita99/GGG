using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostDisplay : MonoBehaviour
{
    private SpriteNumbers SpriteNumbers;
    private SpriteRenderer spriteRenderer;
    private CribmateManager cMManager;
    void Start()
    {
        SpriteNumbers = SpriteNumbers.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        cMManager = transform.parent.GetComponent<CribmateManager>();
        spriteRenderer.sprite = SpriteNumbers.numbers[cMManager.stats.cost];
    }
}
