using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUpState : CrawlerBaseState
{

    public float hitstunTimer;
    public float getUpLength;
    public override void EnterState(CrawlerStateManager crawler, float number = 0.0f, string str = "")
    {
        hitstunTimer = 0f;
    }

    public override void UpdateState(CrawlerStateManager crawler)
    {
        hitstunTimer += Time.deltaTime;
        if (hitstunTimer >= getUpLength)
        {
            crawler.SwitchState(crawler.CrawlingState);
        }
    }



    public override void OnTriggerStay(CrawlerStateManager crawler, Collider2D collider)
    {

    }

    public override void OnTriggerExit(CrawlerStateManager crawler, Collider2D collider)
    {

    }
    public override void ForcedOutOfState(CrawlerStateManager crawler)
    {

    }
}
