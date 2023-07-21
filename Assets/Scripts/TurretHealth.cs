using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    private Vector2 originalPosition;
    public float shakeDuration = 0.15f;
    public float shakeIntensity = 0.1f;

    private void Start()
    {
        originalPosition = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // ? ISSUE
        // You can attack two at the same time and there is friendly turret attack
        if (collision.gameObject.layer.Equals(6))
        {
            // Debug.Log("destroyed");
            health -= 10;
            Shake();
        }
        else if (collision.gameObject.layer.Equals(7))
        {
            // Debug.Log("destroyed");
            health -= 20;
            Shake();
        }

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }


    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        // Save the starting time of the shake
        float startTime = Time.time;

        while (Time.time < startTime + shakeDuration)
        {
            // Generate random offsets for the shake effect
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

            // Apply the shake effect to the object's position
            transform.position = originalPosition + new Vector2(offsetX, offsetY);

            yield return null; // Wait for the next frame
        }

        // Set the transform back to its original position after the shake
        transform.position = originalPosition;
    }


}
