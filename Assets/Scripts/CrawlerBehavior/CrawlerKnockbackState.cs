using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerKnockbackState : CrawlerBaseState
{

    public float hitstunTimer;
    public float knockbackSpeed;
    public override void EnterState(CrawlerStateManager crawler, float number = 0.0f, string str = "")
    {


        
    }

    public override void UpdateState(CrawlerStateManager crawler)
    {


        crawler.timer -= Time.deltaTime * knockbackSpeed;
        knockbackSpeed -= Time.deltaTime * 10;
        if (knockbackSpeed <= 0)
        {
            crawler.SwitchState(crawler.GetUpState);
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
