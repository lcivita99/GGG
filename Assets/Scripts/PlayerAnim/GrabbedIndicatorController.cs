using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedIndicatorController : MonoBehaviour
{
    private CombatStateManager combateStateManager;

    private float grabbedTimer;

    private float holdLength;

    private Vector3 targetLocalScale;

    void Start()
    {
        combateStateManager = transform.parent.GetComponent<CombatStateManager>();
        holdLength = combateStateManager.holdLength;
        transform.localScale = Vector3.zero;
        targetLocalScale = Vector3.zero;
        grabbedTimer = 0;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        targetLocalScale = Vector3.zero;
        grabbedTimer = 0;
        InvokeRepeating("UpdateScale", 0, 0.05f);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        targetLocalScale = Vector3.zero;
        grabbedTimer = 0;

        CancelInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        grabbedTimer += Time.deltaTime;
        if (grabbedTimer <= holdLength * 0.07f)
        {
            targetLocalScale = grabbedTimer / (holdLength * 0.07f) * Vector3.one;
        }
        else if (grabbedTimer <= holdLength * 0.8f)
        {
            targetLocalScale = Vector3.one * Mathf.Sin(grabbedTimer - (holdLength * 0.07f)) * 0.25f + Vector3.one;
        }
        else
        {
            targetLocalScale -= Vector3.one * Time.deltaTime * holdLength;
        }

        if (combateStateManager.currentState != combateStateManager.GrabbedState)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateScale()
    {
        transform.localScale = targetLocalScale;
    }
}
