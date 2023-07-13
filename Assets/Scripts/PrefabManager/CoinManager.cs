using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private Collider2D coinCollider;

    private Animator coinAnim;

    private float timer;
    private float timeToDespawn = 6;

    private bool isDespawning;

    private float activationDelay = 0.5f;
    private bool coinActivated;

    void Start()
    {
        coinCollider = GetComponent<Collider2D>();
        coinAnim = GetComponent<Animator>();
        timer = 0;

        isDespawning = false;

        coinCollider.enabled = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= activationDelay && !coinActivated)
        {
            coinActivated = true;
            coinCollider.enabled = true;
        }

        if (timer >= timeToDespawn && !isDespawning)
        {
            isDespawning = true;
            coinAnim.SetTrigger("despawn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(3)) // player
        {
            collision.GetComponent<CurrencyManager>().ChangeCurrency(1);
            coinCollider.enabled = false;
            coinAnim.SetTrigger("despawn");
        }
        else if (collision.gameObject.layer.Equals(11)) // invulnerable (in case you grab it when invulnerable
        {
            collision.transform.parent.GetComponent<CurrencyManager>().ChangeCurrency(1);
            coinCollider.enabled = false;
            coinAnim.SetTrigger("despawn");
        }
    }
}
