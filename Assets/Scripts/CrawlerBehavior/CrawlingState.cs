using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingState : CrawlerBaseState
{
    public override void EnterState(CrawlerStateManager crawler, float number = 0.0f, string str = "")
    {
        crawler.spriteAnim.SetTrigger("crawl");
    }

    public override void UpdateState(CrawlerStateManager crawler)
    {
        if (crawler.goingBackwards)
        {
            if (crawler.timer >= 0) crawler.timer -= Time.deltaTime;
        }
        else crawler.timer += Time.deltaTime * crawler.speedBuff;
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            crawler.SwitchState(crawler.KnockbackState);
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
