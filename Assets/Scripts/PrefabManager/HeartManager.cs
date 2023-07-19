using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    private Collider2D heartCollider;

    private Animator heartAnim;

    private float timer;
    private float timeToDespawn = 15;

    private float healthGiven = 20;

    private bool isDespawning;

    private float activationDelay = 0.3f;
    private bool heartActivated;

    void Start()
    {
        heartCollider = GetComponent<Collider2D>();
        heartAnim = GetComponent<Animator>();
        timer = 0;

        isDespawning = false;

        heartCollider.enabled = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= activationDelay && !heartActivated)
        {
            heartActivated = true;
            heartCollider.enabled = true;
        }

        if (timer >= timeToDespawn && !isDespawning)
        {
            isDespawning = true;
            GridManager.instance.RemoveFromGrid(transform.position);
            heartCollider.enabled = false;
            heartAnim.SetTrigger("despawn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(3)) // player
        {
            CombatStateManager combat = collision.GetComponent<CombatStateManager>();

            if (combat.health >= 100 - healthGiven)
            {
                combat.health = 100;
            } else
            {
                combat.health += 20;
            }

            combat.healthBarVisuals.UpdateUI();

            GridManager.instance.RemoveFromGrid(transform.position);
            heartCollider.enabled = false;
            heartAnim.SetTrigger("despawn");
        }
        else if (collision.gameObject.layer.Equals(11)) // invulnerable (in case you grab it when invulnerable
        {
            CombatStateManager combat = collision.GetComponent<CombatStateManager>();

            if (combat.health >= 100 - healthGiven)
            {
                combat.health = 100;
            }
            else
            {
                combat.health += 20;
            }

            combat.healthBarVisuals.UpdateUI();

            GridManager.instance.RemoveFromGrid(transform.position);
            heartCollider.enabled = false;
            heartAnim.SetTrigger("despawn");
        }
    }
}
