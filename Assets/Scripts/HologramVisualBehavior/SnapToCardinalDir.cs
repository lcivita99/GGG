using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToCardinalDir : MonoBehaviour
{
    private Vector3[] dirs = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
    private int dirIdx;
    void Start()
    {
        dirIdx = 0;
        InvokeRepeating("Rotate", 0.3f, 0.3f);

        transform.up = dirs[dirIdx];
    }

    private void Rotate()
    {
        if (dirIdx < dirs.Length - 1)
        {
            dirIdx++;
        } else
        {
            dirIdx = 0;
        }

        transform.up = dirs[dirIdx];
    }

    
}
