using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleComponent : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2.0f;
    [SerializeField] private int damageAmount = 1;
    [Tooltip("How fast do you want this thing to go")]
    [SerializeField] private float objectVelocityMultiplier = 100f;
    [Tooltip("What direction do you want this thing to go?")]
    [SerializeField] private Vector2 objectVelocity;

    public void SetObjectVelocity(Vector2 velocity)
    {
        objectVelocity.x = velocity.x;
        objectVelocity.y = velocity.y;  
    }

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = objectVelocity * objectVelocityMultiplier;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject && collision.gameObject.GetComponent<PlayerHealth>())
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(!playerHealth)
            {
                return;
            }
            if(playerHealth.CanTakeDamage)
            {
                playerHealth.AdjustHealth(-damageAmount);
            }
            Destroy(gameObject);
        }
        else if(collision.gameObject && collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        Destroy(gameObject, destroyTime);
    }
}
