using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerSpriteAnim : MonoBehaviour
{
    [SerializeField] private Transform crawlerTransform;
    private CrawlerStateManager crawlerStateManager;

    [SerializeField] private LayerMask wallMask;

    public Vector2 nextHeadTarget;
    public Vector2 nextArmBackTarget;
    public Vector2 nextArmForwardTarget;
    public Vector2 nextLegBackTarget;
    public Vector2 nextLegForwardTarget;

    [SerializeField] private Transform headIKTarget;
    [SerializeField] private Transform armBackIKTarget;
    [SerializeField] private Transform armForwardIKTarget;
    [SerializeField] private Transform legBackIKTarget;
    [SerializeField] private Transform legForwardIKTarget;

    [SerializeField] private Transform hipsTransform;
    [SerializeField] private Transform headTransform;

    private float[] directions;

    
    void Start()
    {
        crawlerStateManager = crawlerTransform.GetComponent<CrawlerStateManager>();
        directions = new float[] { hipsTransform.eulerAngles.z, hipsTransform.eulerAngles.z + 90, hipsTransform.eulerAngles.z + 180 };

        nextHeadTarget = headIKTarget.position;
        nextArmBackTarget = armBackIKTarget.position;
        nextArmForwardTarget = armForwardIKTarget.position;
        nextLegBackTarget = legBackIKTarget.position;
        nextLegForwardTarget = legForwardIKTarget.position;
        SetTransform();
    }

    // Update is called once per frame
    void Update()
    {
        SetTransform();


        // TODO make this lerp and only change when there is a phase mismatch.
        hipsTransform.transform.eulerAngles = new Vector3(0, 0, directions[crawlerStateManager.phase]);



        //if (Vector2.Distance(armForwardIKTarget.position, nextArmForwardTarget) > 0.5f) armForwardIKTarget.position = nextArmForwardTarget;

        UpdateIKToTarget(armForwardIKTarget, nextArmForwardTarget, 0.3f);
        UpdateIKToTarget(armBackIKTarget, nextArmBackTarget, 0.4f);
        UpdateIKToTarget(legForwardIKTarget, nextLegForwardTarget, 0.5f);
        UpdateIKToTarget(legBackIKTarget, nextLegBackTarget, 0.6f);

        // top
        if (crawlerStateManager.phase == 0)
        {
            Vector2 headTargetPos;
            bool transitioning;
            RaycastNextTarget(crawlerTransform.position + Vector3.left, Vector2.down, Vector2.down + Vector2.right/2, out headTargetPos, out transitioning);
            if (!transitioning)
            {
                headIKTarget.position = headTargetPos + Vector2.up / 2;
            }
            else
            {
                headIKTarget.position = headTargetPos + Vector2.left / 2;
            }
            

            // arms
            RaycastNextTarget(headTransform.position, Vector2.down + Vector2.left/2, Vector2.down + Vector2.right / 2, out nextArmForwardTarget);
            RaycastNextTarget(headTransform.position, Vector2.down + Vector2.left/2, Vector2.down + Vector2.right / 2, out nextArmBackTarget);

            // legs
            RaycastNextTarget(hipsTransform.position, Vector2.down + Vector2.left/2, Vector2.down + Vector2.right / 2, out nextLegForwardTarget);
            RaycastNextTarget(hipsTransform.position, Vector2.down + Vector2.left/2, Vector2.down + Vector2.right / 2, out nextLegBackTarget);

        }
        // side
        else if (crawlerStateManager.phase == 1)
        {
            Vector2 headTargetPos;
            bool transitioning;
            RaycastNextTarget(headIKTarget.position, Vector2.right, Vector2.right + Vector2.up / 2, out headTargetPos, out transitioning);
            if (!transitioning)
            {
                headIKTarget.position = headTargetPos + Vector2.left / 2;
            } else
            {
                headIKTarget.position = headTargetPos + Vector2.down / 2;
            }

            // arms
            RaycastNextTarget(headTransform.position, Vector2.right + Vector2.down / 2, Vector2.right + Vector2.up / 2, out nextArmForwardTarget);
            RaycastNextTarget(headTransform.position, Vector2.right + Vector2.down / 2, Vector2.right + Vector2.up / 2, out nextArmBackTarget);

            // legs
            RaycastNextTarget(hipsTransform.position, Vector2.right + Vector2.down / 2, Vector2.right + Vector2.up / 2, out nextLegForwardTarget);
            RaycastNextTarget(hipsTransform.position, Vector2.right + Vector2.down / 2, Vector2.right + Vector2.up / 2, out nextLegBackTarget);
        }
        // bottom
        else
        {
            Vector2 headTargetPos;
            RaycastNextTarget(headIKTarget.position, Vector2.up, Vector2.up + Vector2.left / 2, out headTargetPos);
            if (headTargetPos != Vector2.zero) headIKTarget.position = headTargetPos + Vector2.down;

            // arms
            RaycastNextTarget(headTransform.position, Vector2.up + Vector2.right / 2, Vector2.up, out nextArmForwardTarget);
            RaycastNextTarget(headTransform.position, Vector2.up + Vector2.right / 2, Vector2.up, out nextArmBackTarget);

            // legs
            RaycastNextTarget(hipsTransform.position, Vector2.up + Vector2.right / 2, Vector2.up, out nextLegForwardTarget);
            RaycastNextTarget(hipsTransform.position, Vector2.up + Vector2.right / 2, Vector2.up, out nextLegBackTarget);
        }
    }

    private void SetTransform()
    {
        if (crawlerStateManager.phase == 0)
        {
            hipsTransform.position = crawlerTransform.position - Vector3.up / 8 + Vector3.right / 2;
        }
        else if (crawlerStateManager.phase == 1)
        {
            hipsTransform.position = crawlerTransform.position + Vector3.right / 8;
        } else
        {
            hipsTransform.position = crawlerTransform.position + Vector3.up / 8;
        }

    }

    private void RaycastNextTarget(Vector2 origin, Vector2 direction, Vector2 altDirection, out Vector2 nextTarget)
    {
        bool transitioning;
        RaycastNextTarget(origin, direction, altDirection, out nextTarget, out transitioning);
    }

    private void RaycastNextTarget(Vector2 origin, Vector2 direction, Vector2 altDirection, out Vector2 nextTarget, out bool transitioning)
    {
        transitioning = false;
        
        // Cast a ray and get the hit information
        RaycastHit2D hit = Physics2D.Raycast(origin, direction);

        // Check if the raycast hits an object
        if (hit.collider != null)
        {
            nextTarget = hit.point;
        } else
        {
            RaycastHit2D hit2 = Physics2D.Raycast(origin, altDirection);

            transitioning = true;

            if (hit2.collider != null)
            {
                nextTarget = hit2.point;
            } else
            {
                nextTarget = origin + altDirection / 2;
            }
        }

        
    }

    //private void UpdateIKToTarget(Transform IKTarget, Vector2 nextTarget, float distReq)
    //{
    //    if (Vector2.Distance(IKTarget.position, nextTarget) > distReq)
    //    {
    //        //Debug.Log(Vector2.Distance(IKTarget.position, nextTarget));
    //        IKTarget.position = nextTarget;
    //    }
    //}

    private float interpolationDuration = 0.5f;

    private class InterpolationData
    {
        public bool isInterpolating = false;
        public float interpolationTimer = 0f;
        public Vector2 initialPosition;
        public Vector2 targetPosition;
    }

    private Dictionary<Transform, InterpolationData> interpolationDataDict = new Dictionary<Transform, InterpolationData>();

    private void UpdateIKToTarget(Transform IKTarget, Vector2 nextTarget, float distReq)
    {
        // Check if the object has interpolation data
        if (!interpolationDataDict.ContainsKey(IKTarget))
        {
            interpolationDataDict[IKTarget] = new InterpolationData();
        }

        InterpolationData interpolationData = interpolationDataDict[IKTarget];

        if (!interpolationData.isInterpolating && Vector2.Distance(IKTarget.position, nextTarget) > distReq)
        {
            interpolationData.initialPosition = IKTarget.position;
            interpolationData.targetPosition = nextTarget;
            interpolationData.interpolationTimer = 0f;
            interpolationData.isInterpolating = true;
        }

        if (interpolationData.isInterpolating)
        {
            interpolationData.interpolationTimer += Time.deltaTime;

            float t = Mathf.Clamp01(interpolationData.interpolationTimer / interpolationDuration);

            IKTarget.position = Vector2.Lerp(interpolationData.initialPosition, interpolationData.targetPosition, t);

            if (t >= 1f)
            {
                interpolationData.isInterpolating = false;
            }
        }
    }
}
