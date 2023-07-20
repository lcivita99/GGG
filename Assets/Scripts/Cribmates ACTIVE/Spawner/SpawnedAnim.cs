using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedAnim : MonoBehaviour
{
    [SerializeField] private Rigidbody2D spawnedRb;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (spawnedRb.velocity.magnitude > 0.1f)
        {
            if (!anim.GetBool("crawling"))
            {
                anim.SetBool("crawling", true);
            }
            
        } else
        {
            if (anim.GetBool("crawling"))
            {
                anim.SetBool("crawling", false);
            }
        }
    }
}
