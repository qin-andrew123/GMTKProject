using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamagingZone : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject && collision.gameObject.GetComponent<PlayerHealth>())
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (!playerHealth)
            {
                return;
            }
            if (playerHealth.CanTakeDamage)
            {
                playerHealth.AdjustHealth(-damageAmount);
            }
        }
    }
}
