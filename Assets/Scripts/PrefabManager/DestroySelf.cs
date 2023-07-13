using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public void DestroyInstanceSelf()
    {
        Destroy(gameObject);
    }
}
