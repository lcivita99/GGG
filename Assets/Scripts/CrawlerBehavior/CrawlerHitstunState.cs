using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerHitstunState : CrawlerBaseState
{

    public float hitstunTimer;
    public float hitstunLength;
    public override void EnterState(CrawlerStateManager crawler, float number = 0.0f, string str = "")
    {
        hitstunTimer = 0f;
        crawler.spriteAnim.SetTrigger("hitstun");
    }

    public override void UpdateState(CrawlerStateManager crawler)
    {
        hitstunTimer += Time.deltaTime;
        if (hitstunTimer >= hitstunLength)
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