using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed = 7f;

    private float bulletTimer;
    private float timeToDespawn = 1.5f;
    private float bulletShieldStunLength = 0.1f;

    private Rigidbody2D rb;

    private Vector3 shotDir;

    [SerializeField] private Transform turret;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localPosition = turret.transform.localPosition + turret.transform.up/2;
        bulletTimer = 0;
        shotDir = turret.up;

        rb.velocity = shotDir * bulletSpeed;
    }

    void Update()
    {
        bulletTimer += Time.deltaTime;

        if (bulletTimer >= timeToDespawn)
        {
            bulletTimer = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(3)) // player
        {
            if (IsEnemyPlayer(collision)) // what kind of player? 
            {
                // get combat state manager:
                CombatStateManager combat = collision.GetComponent<CombatStateManager>();
                // calculate direction: 

                //
                
                // calculate vector 2: 
                Vector2 dir = (collision.transform.position -transform.position).normalized;
                if (combat.currentState == combat.ShieldState || combat.currentState == combat.ShieldStunState)
                {
                    combat.SwitchState(combat.ShieldStunState, bulletShieldStunLength, "", dir);
                } else
                {
                    combat.currentState.ForcedOutOfState(combat);
                    combat.SwitchState(combat.HitstunState, 0, "bullet", dir);
                }
                bulletTimer = 0;
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.layer.Equals(10)) // wall
        {
            bulletTimer = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
        // TODO instantiate bullet death
    }

    private bool IsEnemyPlayer (Collider2D collision)
    {
        int bulletTeam = turret.gameObject.GetComponent<PlaceableObj>().myTeam;
        int colliderTeam = collision.gameObject.GetComponent<PlayerMovement>().team;
        if (bulletTeam == colliderTeam)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
