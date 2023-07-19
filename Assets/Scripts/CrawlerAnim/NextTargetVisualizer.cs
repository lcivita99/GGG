using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTargetVisualizer : MonoBehaviour
{
    [SerializeField] private int ID;

    private CrawlerSpriteAnim parentScript;
    void Start()
    {
        parentScript = transform.parent.GetComponent<CrawlerSpriteAnim>();

        if (ID == 1)
        {
            transform.position = parentScript.nextArmBackTarget;
        } else if (ID == 2)
        {
            transform.position = parentScript.nextArmForwardTarget;
        } else if (ID == 3)
        {
            transform.position = parentScript.nextLegBackTarget;
        } else if (ID == 4)
        {
            transform.position = parentScript.nextLegForwardTarget;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ID == 1)
        {
            transform.position = parentScript.nextArmBackTarget;
        }
        else if (ID == 2)
        {
            transform.position = parentScript.nextArmForwardTarget;
        }
        else if (ID == 3)
        {
            transform.position = parentScript.nextLegBackTarget;
        }
        else if (ID == 4)
        {
            transform.position = parentScript.nextLegForwardTarget;
        }
    }
}
